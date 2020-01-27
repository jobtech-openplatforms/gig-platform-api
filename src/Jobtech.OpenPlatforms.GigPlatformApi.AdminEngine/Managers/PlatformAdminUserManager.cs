using AF.GigPlatform.Connectivity.Models;
using AF.GigPlatform.Core.Entities;
using AF.GigPlatform.Core.Exceptions;
using AF.GigPlatform.Core.ValueObjects;
using AF.GigPlatform.Store.Config;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AF.GigPlatform.AdminEngine.Managers
{
    public class PlatformAdminUserManager : IPlatformAdminUserManager
    {
        private IDocumentStore _documentStore;

        public PlatformAdminUserManager(IDocumentStoreHolder documentStore)
        {
            _documentStore = documentStore.Store;
        }

        //public PlatformAdminUser GetUser(PlatformAdminUserId userId)
        //{
        //    using (IDocumentSession session = _documentStore.OpenSession())
        //    {
        //        return session.Load<PlatformAdminUser>(userId.Value);
        //    }
        //}

        public async Task<PlatformAdminUser> GetUserAsync(PlatformAdminUserId userId)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<PlatformAdminUser>(userId.Value);
            }
        }

        public async Task<PlatformAdminUser> GetOrCreateUserAsync(string uniqueIdentifier, IAsyncDocumentSession session)
        {
            var existingUser = await session.Query<PlatformAdminUser>().SingleOrDefaultAsync(u => u.UniqueIdentifier == uniqueIdentifier);
            if (existingUser != null)
            {
                return existingUser;
            }

            var newUser = new PlatformAdminUser
            {
                UniqueIdentifier = uniqueIdentifier
            };

            await session.StoreAsync(newUser);
            return newUser;
        }

        public async Task<PlatformAdminUser> GetByUniqueIdentifierAsync(string uniqueIdentifier,
            IAsyncDocumentSession session)
        {
            return await session.Query<PlatformAdminUser>().SingleOrDefaultAsync(u => u.UniqueIdentifier == uniqueIdentifier);
        }

        public async Task<PlatformAdminUser> GetUserAsync(string email)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.Query<PlatformAdminUser>().FirstOrDefaultAsync(p => p.Email == email);
            }
        }

        public async Task<PlatformAdminUser> SaveUserAsync(PlatformAdminUser user)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var exists = await session
                    .Query<PlatformAdminUser>()
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

        public async Task<PlatformAdminUser> UpdateUserAsync(PlatformAdminUser user)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ApiException("You need a valid email, friend.");
            }
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var exists = await session
                    .Query<PlatformAdminUser>()
                    .Where(x => x.Email == user.Email)
                    .AnyAsync()
                    ;
                if (!exists)
                {
                    throw new ApiException("User not found.");
                }

                var existingUser = await session
                    .Query<PlatformAdminUser>()
                    .Where(x => x.Email == user.Email)
                    .FirstOrDefaultAsync()
                    ;
                existingUser.Email = user.Email;
                existingUser.Name = user.Name;
                existingUser.PasswordHash = user.PasswordHash ?? existingUser.PasswordHash;
                existingUser.PasswordSalt = user.PasswordSalt ?? existingUser.PasswordSalt;

                await session.StoreAsync(user);
                await session.SaveChangesAsync();

                return user;
            }
        }

        public async Task<IEnumerable<PlatformAdminUser>> GetAllAsync()
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                if (await session
                    .Query<PlatformAdminUser>()
                    .AnyAsync())
                    return await session
                        .Query<PlatformAdminUser>()
                        .ToListAsync()
                        ;

                return new List<PlatformAdminUser>();
            }
        }

        public async Task<PlatformAdminUser> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var user = await session
                    .Advanced.AsyncDocumentQuery<PlatformAdminUser>()
                    .WhereEquals(x => x.Email, username).Take(1)
                    .FirstOrDefaultAsync();

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

        public async Task<PlatformAdminUser> CreateAsync(PlatformAdminUser user, string password)
        {
            // validation
            if (user == null)
                throw new ApiException("PlatformAdminUser account details are missing.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ApiException("Password is required");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ApiException("Email is required");

            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var existing = await session.Query<PlatformAdminUser>().Take(1).FirstOrDefaultAsync(x => x.Email == user.Email);
                if (existing != null)
                    throw new ApiException("Username \"" + user.Email + "\" is already taken");
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return await SaveUserAsync(user);
        }

        public async Task<string> PasswordResetCodeAsync(PlatformAdminUserId userId)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<PlatformAdminUser>(userId.Value);
                if (user == null)
                    throw new ApiException("No account found with ID \"" + userId.Value + "\".");
                var guid = Guid.NewGuid().ToString();
                user.PasswordReset = user.PasswordReset ?? new PasswordReset();
                user.PasswordReset.Requested = DateTimeOffset.UtcNow;
                user.PasswordReset.ResetCode = guid;

                await session.SaveChangesAsync();

                return guid;
            }
        }

        public async Task ResetLoginAsync(string email, string password)
        {
            PlatformAdminUser user = null;

            if (string.IsNullOrWhiteSpace(password))
                throw new ApiException("Password is required");

            if (string.IsNullOrWhiteSpace(email))
                throw new ApiException("Email is required");

            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                user = await session.Query<PlatformAdminUser>().Take(1).FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                    throw new ApiException("No account found with email \"" + email + "\".");
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await UpdateAsync(user, password);
        }

        public async Task UpdateAsync(PlatformAdminUser userParam, string password = null)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<PlatformAdminUser>(((PlatformAdminUserId)userParam.Id).Value);

                if (user == null)
                    throw new ApiException("PlatformAdminUser not found");

                if (userParam.Email != user.Email)
                {
                    // username has changed so check if the new username is already taken
                    if (await session.Query<PlatformAdminUser>().AnyAsync(x => x.Email == userParam.Email))
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

        public async Task DeleteAsync(PlatformAdminUserId id)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<PlatformAdminUser>(id.Value);
                if (user != null)
                {
                    session.Delete(user);
                    await session.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<Platform>> GetPlatformsAsync(PlatformAdminUserId id)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var admin = await session.LoadAsync<PlatformAdminUser>(id.Value);
                var platforms = await session
                    .Query<Project>()
                    .Where(p => p.AdminIds.Any(a => a == id.Value) || p.OwnerAdminId == id)
                    .Select(p => new Platform { ExportDataUri = p.Platforms.First().ExportDataUri, Id = p.Platforms.First().Id, PlatformToken = p.Platforms.First().PlatformToken })
                    .ToListAsync();
                //var platforms = await session.Query<Platform>().Where(x => x.Admins.Any(a => a.Id == id.Value)).ToListAsync();
                return platforms;
            }
        }

        //public async Task<Platform> GetAdminPlatformAsync(PlatformAdminUserId id)
        //{
        //    using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
        //    {
        //        var admin = await session.LoadAsync<PlatformAdminUser>(id.Value);
        //        var platforms = await session.Query<Platform>().Where(x => x.Admins.Any(a => a.Id == id.Value)).FirstOrDefaultAsync();
        //        return platforms;
        //    }
        //}

        public async Task<PlatformAdminUser> UpdateContactAsync(PlatformAdminUserId id, PlatformUpdateContactRequestModel contactUpdate)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<PlatformAdminUser>(id.Value);
                if (user == null)
                {
                    throw new ApiException("No such admin, friend.");
                }
                user.Email = contactUpdate.Email ?? user.Email;
                user.Name = contactUpdate.Name ?? user.Name;

                //var adminForThesePlatforms = await session.Query<Platform>().Where(x => x.Admins.Any(a => a.Id == user.Id)).ToListAsync();

                // Check for corrupt data, since it's not a relational database
                // TODO: This should actually be in a dedicated method

                //foreach (var platform in adminForThesePlatforms
                //    .Where(w => user.Platforms != null && !user.Platforms.Any(a => w.Id == a.Id) /* Where there aren't any in the PlatformAdminUser.Platforms */)
                //    //.SelectMany(s => s.Id /* Select the Platform.Id */)
                //    .ToList()
                //    )
                //{
                //    // This platform was missing from the admin user entry - add it
                //    user.Platforms.Add(platform);
                //}

                ////foreach (var platform in user.Platforms)
                ////{
                ////    foreach (var admin in platform.Admins)
                ////    {
                ////        admin.Name = contactUpdate.Name;
                ////        admin.Email = contactUpdate.Email;
                ////    }
                ////}

                //if (user.Platforms != null && user.Platforms.Any())
                //{
                //    string[] userPlatformIds = user.Platforms.Select(p => p.Id).ToList().ToArray();
                //    //var platformsMissingThisAdmin = await session.Query<Platform>()
                //    //    .Where(w => userPlatformIds.Contains(w.Id))
                //    //    .ToListAsync();

                //    foreach (var platformId in userPlatformIds)
                //    {
                //        var platform = await session.LoadAsync<Platform>(platformId);
                //        //if (!platform.Admins.Any(a => a.Id == user.Id))
                //        //{
                //        //    platform.Admins.Add(user as PlatformAdmin);
                //        //} else
                //        //{
                //        //    var adm = platform.Admins.Where(a => a.Id == user.Id).FirstOrDefault();
                //        //    adm.Email = contactUpdate.Email ?? adm.Email;
                //        //    adm.Name = contactUpdate.Name ?? adm.Name;
                //        //}
                //    }
                //}

                await session.SaveChangesAsync();

                return user;
            }
        }

        #region private helper methods

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

        #endregion private helper methods
    }
}