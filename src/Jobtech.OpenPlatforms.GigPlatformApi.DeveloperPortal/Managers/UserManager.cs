using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Jobtech.OpenPlatforms.GigPlatformApi.Store.Config;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Managers
{
    public class UserManager : IUserManager
    {
        private IDocumentStore _documentStore;

        public UserManager(IDocumentStoreHolder documentStore)
        {
            _documentStore = documentStore.Store;
        }

        public User GetUser(UserId userId)
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                return session.Load<User>(userId);
            }
        }

        public async Task<User> GetUserAsync(UserId userId)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<User>(userId);
            }
        }

        public async Task<User> SaveUserAsync(User user)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var exists = await session
                    .Query<User>()
                    .Where(x => x.Email == user.Email)
                    .AnyAsync()
                    ;
                if (exists)
                {
                    throw new ApiException("A user with that email already exists.");
                }
                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    throw new ApiException("You need a valid email, friend.");
                }

                await session.StoreAsync(user);
                await session.SaveChangesAsync();

                return user;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                if (await session
                    .Query<User>()
                    .AnyAsync())
                    return await session
                        .Query<User>()
                        .ToListAsync()
                        ;

                return new List<User>();
            }
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var user = await session.Query<User>().SingleOrDefaultAsync(x => x.Email == username);

                // check if username exists
                if (user == null)
                    return null;

                // check if password is correct
                if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                    return null;

                // authentication successful
                return user;
            }
        }

        public async Task<User> CreateAsync(User user, string password)
        {
            // validation
            if (user==null)
                throw new ApiException("User account details are missing.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ApiException("Password is required");

            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                if (await session.Query<User>().AnyAsync(x => x.Email != string.Empty && x.Email == user.Email))
                    throw new ApiException("Username \"" + user.Email + "\" is already taken");
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return await SaveUserAsync(user);
        }

        public async Task UpdateAsync(User userParam, string password = null)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var user = await GetUserAsync(userParam.Id);

                if (user == null)
                    throw new ApiException("User not found");

                if (userParam.Email != user.Email)
                {
                    // username has changed so check if the new username is already taken
                    if (await session.Query<User>().AnyAsync(x => x.Email == userParam.Email))
                        throw new ApiException("Username " + userParam.Email + " is already taken");
                }

                // update user properties
                user.Name = userParam.Name;
                user.Email = userParam.Email;

                // update password if it was entered
                if (!string.IsNullOrWhiteSpace(password))
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(password, out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                }

                await session.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(UserId id)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(id);
                if (user != null)
                {
                    session.Delete(user);
                    await session.SaveChangesAsync();
                }
            }
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}