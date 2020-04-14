using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Managers {
    public interface IProjectUpdateManager {
        Task<Project> Update (UpdateProjectRequest request, IAsyncDocumentSession session, PlatformAdminUserId userId);
    }
}