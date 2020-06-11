using System.Collections.Generic;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers
{
    public interface IPlatformAdminUserManager
    {
        //PlatformAdminUser GetUser(PlatformAdminUserId userId);
        //Task<PlatformAdminUser> GetUserAsync(string email, IAsyncDocumentSession session);
        //Task<PlatformAdminUser> GetUserAsync(PlatformAdminUserId userId, IAsyncDocumentSession session);
        Task<PlatformAdminUser> GetOrCreateUserAsync(string uniqueIdentifier, IAsyncDocumentSession session);

        Task<PlatformAdminUser> GetByUniqueIdentifierAsync(string uniqueIdentifier,
            IAsyncDocumentSession session);
        //Task<PlatformAdminUser> SaveUserAsync(PlatformAdminUser user, IAsyncDocumentSession session);
        //Task<PlatformAdminUser> CreateAsync(PlatformAdminUser user, string password, IAsyncDocumentSession session);
        //Task ResetLoginAsync(string email, string password, IAsyncDocumentSession session);
        //Task<string> PasswordResetCodeAsync(PlatformAdminUserId userId, IAsyncDocumentSession session);
        //Task<IEnumerable<PlatformAdminUser>> GetAllAsync(IAsyncDocumentSession session);
        //Task<PlatformAdminUser> Authenticate(string username, string password, IAsyncDocumentSession session);
        //Task UpdateAsync(PlatformAdminUser userParam, string password = null, IAsyncDocumentSession session);
        //Task DeleteAsync(PlatformAdminUserId id, IAsyncDocumentSession session);
        //Task<IEnumerable<Platform>> GetPlatformsAsync(PlatformAdminUserId id, IAsyncDocumentSession session);
        //Task<PlatformAdminUser> UpdateContactAsync(PlatformAdminUserId id, PlatformUpdateContactRequestModel contactUpdate, IAsyncDocumentSession session);
        //Task<Platform> GetAdminPlatformAsync(PlatformAdminUserId id);
    }
}