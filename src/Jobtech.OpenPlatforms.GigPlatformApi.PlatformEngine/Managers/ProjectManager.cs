﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Jobtech.OpenPlatforms.GigPlatformApi.Store.Config;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers
{
    public class ProjectManager : StoreManager<Project>, IProjectManager
    {
        public ProjectManager(IDocumentStoreHolder documentStore)
            : base(documentStore)
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

        public async Task<TestProject> GetTestByLiveId(ProjectId projectId, IAsyncDocumentSession session)
            => await session.Query<TestProject>()
                            .FirstOrDefaultAsync(p => p.LiveProjectId == projectId.Value);


        public async Task<Project> Update(Project project)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var p = await session.LoadAsync<Project>(project.Id);
                p.LogoUrl = project.LogoUrl;
                p.Name = project.Name;
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
}