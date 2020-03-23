using System.Collections.Generic;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers
{
    public interface IProjectManager : IStoreManager<Project>
    {
        Task<IEnumerable<Project>> GetAll(PlatformAdminUserId userId, IAsyncDocumentSession session);
        Task<IEnumerable<TestProject>> GetAllTest(PlatformAdminUserId userId, IAsyncDocumentSession session);
        Task<Project> Update(Project project);
        Task<TestProject> GetTest(TestProjectId projectId, IAsyncDocumentSession session);
        Task<TestProject> GetTestFromLiveId(ProjectId projectId, IAsyncDocumentSession session);
    }
}
