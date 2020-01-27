using AF.GigPlatform.Core.Entities;
using AF.GigPlatform.Connectivity.Models;
using AF.GigPlatform.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;

namespace AF.GigPlatform.AdminEngine.Managers
{
    public interface IPlatformAdminUserManager
    {
        //PlatformAdminUser GetUser(PlatformAdminUserId userId);
        Task<PlatformAdminUser> GetUserAsync(string email);
        Task<PlatformAdminUser> GetUserAsync(PlatformAdminUserId userId);
        Task<PlatformAdminUser> GetOrCreateUserAsync(string uniqueIdentifier, IAsyncDocumentSession session);

        Task<PlatformAdminUser> GetByUniqueIdentifierAsync(string uniqueIdentifier,
            IAsyncDocumentSession session);
        Task<PlatformAdminUser> SaveUserAsync(PlatformAdminUser user);
        Task<PlatformAdminUser> CreateAsync(PlatformAdminUser user, string password);
        Task ResetLoginAsync(string email, string password);
        Task<string> PasswordResetCodeAsync(PlatformAdminUserId userId);
        Task<IEnumerable<PlatformAdminUser>> GetAllAsync();
        Task<PlatformAdminUser> Authenticate(string username, string password);
        Task UpdateAsync(PlatformAdminUser userParam, string password = null);
        Task DeleteAsync(PlatformAdminUserId id);
        Task<IEnumerable<Platform>> GetPlatformsAsync(PlatformAdminUserId id);
        Task<PlatformAdminUser> UpdateContactAsync(PlatformAdminUserId id, PlatformUpdateContactRequestModel contactUpdate);
        //Task<Platform> GetAdminPlatformAsync(PlatformAdminUserId id);
    }
}