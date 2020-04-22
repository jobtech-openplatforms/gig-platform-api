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
            _logger.LogInformation("Project update request {@id}", project.Id);

            if (TestProjectId.IsValidIdentity(project.Id))
            {
                return await UpdateTest((TestProject)project, session);
            }

            var p = await session.LoadAsync<Project>(project.Id);

            // _logger.LogDebug("Project before {@p}", p);
            if (ProjectId.IsValidIdentity(project.Id))
            {
                await UpdateTestProjectName(project.Id, project.Name, session);
                p.Name = project.Name;
            }
            p.LogoUrl = project.LogoUrl;
            p.Webpage = project.Webpage;
            p.Description = project.Description;
            p.Applications = project.Applications;
            p.Platforms = project.Platforms;
            p.OwnerAdminId = project.OwnerAdminId;
            p.AdminIds = project.AdminIds;
            await session.SaveChangesAsync();
            // _logger.LogDebug("Project after {@p}", p);
            _logger.LogInformation("Project updated {@id}", p.Id);
            return p;
        }

        private async Task UpdateTestProjectName(string liveProjectId, string projectName, IAsyncDocumentSession session)
        {
            // using var session = _documentStore.OpenAsyncSession();
            var p = await session.Query<TestProject>().Where(tp => tp.LiveProjectId == liveProjectId).FirstOrDefaultAsync();
            _logger.LogInformation("TestProject changing name from {oldName} to {newName}", p.Name, projectName);
            p.Name = projectName;
            await session.SaveChangesAsync();
        }

        private async Task<TestProject> UpdateTest(TestProject project, IAsyncDocumentSession session)
        {
            _logger.LogInformation("TestProject UPDATE: New data {@project}", project);
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