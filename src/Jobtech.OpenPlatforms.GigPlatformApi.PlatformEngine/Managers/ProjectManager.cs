using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers
{
    public class ProjectManager : StoreManager<Project>, IProjectManager
    {
        public ProjectManager(IDocumentStore documentStore, ILogger<ProjectManager> logger)
            : base(documentStore, logger)
        {
        }

        public async Task<IEnumerable<Project>> GetAll(PlatformAdminUserId adminId, IAsyncDocumentSession session)
            => await session.Query<Project>()
                            .Where(p => p.OwnerAdminId == adminId.Value || p.AdminIds.Contains(adminId.Value))
                            .OrderByDescending(p => p.Id)
                            .ToListAsync();


        public async Task<IEnumerable<TestProject>> GetAllTest(PlatformAdminUserId userId, IAsyncDocumentSession session)
            => await session.Query<TestProject>()
                            .Where(p => p.OwnerAdminId == userId.Value || p.AdminIds.Contains(userId.Value))
                            .OrderByDescending(p => p.Id)
                            .ToListAsync();

        public async Task<TestProject> GetTest(TestProjectId projectId, IAsyncDocumentSession session)
            => await session.LoadAsync<TestProject>(projectId.Value);

        public async Task<TestProject> GetTestFromLiveId(ProjectId projectId, IAsyncDocumentSession session)
            => await session.Query<TestProject>().Where(tp => tp.LiveProjectId == projectId.Value).FirstOrDefaultAsync();

        public async Task<Project> Update(Project project, IAsyncDocumentSession session)
        {
            var p = await session.LoadAsync<Project>(project.Id);

            if (p == null)
            {
                // Seems it's not a 
                return await UpdateTest(project, session);
            }
            _logger.LogInformation("Project before {p}", p);
            p.LogoUrl = project.LogoUrl;
            p.Name = project.Name;
            p.Webpage = project.Webpage;
            p.Description = project.Description;
            p.Applications = project.Applications;
            p.Platforms = project.Platforms;
            p.OwnerAdminId = project.OwnerAdminId;
            p.AdminIds = project.AdminIds;
            await session.SaveChangesAsync();
            _logger.LogInformation("Project after {p}", p);
            await UpdateTestProjectName(project, session);
            return p;
        }

        private async Task UpdateTestProjectName(Project project, IAsyncDocumentSession session)
        {
            var p = await session.Query<TestProject>().Where(tp => tp.LiveProjectId == project.Id).FirstOrDefaultAsync();
            _logger.LogInformation("TestProject name change from {oldName} to {newName}", p.Name, project.Name);
            p.Name = project.Name;
            await session.SaveChangesAsync();
            _logger.LogInformation("TestProject name changed to {newName}", p.Name);
        }

        private async Task<Project> UpdateTest(Project project, IAsyncDocumentSession session)
        {
            var p = await session.LoadAsync<TestProject>(project.Id);
            p.LogoUrl = project.LogoUrl;
            // p.Name = project.Name; // Disallow editing project name to be different than live project name
            p.Webpage = project.Webpage;
            p.Description = project.Description;
            p.Applications = project.Applications;
            p.Platforms = project.Platforms;
            p.OwnerAdminId = project.OwnerAdminId;
            p.AdminIds = project.AdminIds;
            await session.SaveChangesAsync();
            return p;
        }
    }
}