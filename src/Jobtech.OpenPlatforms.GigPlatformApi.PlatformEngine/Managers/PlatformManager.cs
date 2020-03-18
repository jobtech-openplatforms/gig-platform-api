using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Jobtech.OpenPlatforms.GigPlatformApi.Store.Config;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers
{
    public class PlatformManager : IPlatformManager
    {
        private readonly IDocumentStore _documentStore;

        public PlatformManager(IDocumentStoreHolder documentStore)
        {
            _documentStore = documentStore.Store;
        }

        public async Task<Platform> AddAdminAsync(Platform platform, PlatformAdminUser admin)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {

                // TODO: Instead of this, just create the session once and destroy on dispose
                var project = await session.Query<Project>().Where(p => p.Platforms.FirstOrDefault().Id == platform.Id).FirstOrDefaultAsync();
                if (project == null)
                {
                    throw new ApiException($"The platform with id {platform.Id} was not found.");
                }
                admin = await session.LoadAsync<PlatformAdminUser>(admin.Id);
                if (!project.AdminIds.Contains(admin.Id) && project.OwnerAdminId != admin.Id)
                {
                    var ids = project.AdminIds.ToList();
                    ids.Add(admin.Id);
                    project.AdminIds = ids;
                }

                await session.SaveChangesAsync();

                return platform;
            }
        }

        public async Task<Platform> GetPlatformAsync(PlatformId id)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var project = await
                    session
                    .Query<Project>()
                    .Where(p => p.Platforms.Any(a => a.Id == id.Value))
                    .FirstOrDefaultAsync();

                if (project != null)
                {
                    return project?
                        .Platforms?
                        .FirstOrDefault();
                }
                var testProject = await
                    session
                    .Query<TestProject>()
                    .Where(p => p.Platforms.Any(a => a.Id == id.Value))
                    .FirstOrDefaultAsync();

                return testProject?
                    .Platforms?
                    .FirstOrDefault();
            }
        }

        public async Task<Platform> GetPlatformByTokenAsync(PlatformToken platformToken)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var project = await
                    session
                    .Query<Project>()
                    .Where(p => p.Platforms.Any(a => a.PlatformToken == platformToken.Value))
                    .FirstOrDefaultAsync();
                return project?
                    .Platforms?
                    .FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Platform>> GetPlatformsAsync()
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var projects = await
                    session
                    .Query<Project>()
                    .Where(w => w.Platforms != null && w.Platforms.Any(a => a.Published))
                    .ToListAsync();
                return projects?
                    .Select(s => s.Platforms.First())
                    .ToList();
            }
        }



        public async Task<Platform> UpdatePlatformAsync(PlatformId id, PlatformRequest platformRequest)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var project = await session
                    .Query<Project>()
                    .Where(p => p.Platforms.Any(a => a.Id == id.Value))
                    .FirstOrDefaultAsync();
                var platform = project.Platforms?.FirstOrDefault();

                if (platform == null)
                {
                    throw new ApiException("I couldn't find that platform. Are you sure it's the right token?", 404);
                }

                if (platform.ExportDataUri == platformRequest.ExportDataUri
                    )
                {
                    // Nothing to update
                    throw new ApiException("That's the same information that I have. Great, so we agree.", 200);
                }
                platform.ExportDataUri = platformRequest.ExportDataUri;
                platform.LastUpdate = DateTime.Now;
                await session.SaveChangesAsync();
                return platform;
            }
        }
    }
}