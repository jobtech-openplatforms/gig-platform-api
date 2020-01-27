using AF.GigPlatform.Connectivity.Models;
using AF.GigPlatform.Core.Entities;
using AF.GigPlatform.Core.ValueObjects;
using AF.GigPlatform.Store.Config;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AF.GigPlatform.PlatformEngine.Managers
{
    public class ApplicationManager : IApplicationManager
    {
        private readonly IDocumentStore _documentStore;

        public ApplicationManager(IDocumentStoreHolder documentStore)
        {
            _documentStore = documentStore.Store;
        }

        public async Task<Application> CreateApplicationAsync(Application entity)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(entity);
                await session.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<Application> GetApplicationAsync(ApplicationId applicationId)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<Application>(applicationId.Value);
            }
        }

        public async Task<IEnumerable<Application>> GetApplicationsAsync()
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.Query<Application>().ToListAsync();
            }
        }

        public async Task<IEnumerable<Application>> GetApplicationsAsync(PlatformAdminUserId platformAdminUserId)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.Query<Project>()
                    .Where(a => a.AdminIds.Any(n => n == platformAdminUserId) || a.OwnerAdminId == platformAdminUserId)
                    .Select(s => new Application {
                        Id =s.Id,
                        AuthCallbackUrl = s.Applications.FirstOrDefault().AuthCallbackUrl,
                        EmailVerificationUrl = s.Applications.First().EmailVerificationUrl,
                        GigDataNotificationUrl = s.Applications.First().GigDataNotificationUrl,
                    })
                    .ToListAsync();
            }
        }

        public async Task<Application> RegisterApplicationAsync(ApplicationRegistrationRequest value)
            => await CreateApplicationAsync(value);

        //public async Task<Application> UpdateApplicationAsync(Application application)
        //{

        //    using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
        //    {
        //        var entity = await session.LoadAsync<Application>(application.Id);

        //        entity.Name = application.Name;
        //        entity.Website = application.Website;
        //        entity.Description = application.Description;

        //        await session.SaveChangesAsync();
        //        return entity;
        //    }
        //}

        //public async Task<Application> UpdateApplicationAdminAsync(Application application)
        //{

        //    using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
        //    {
        //        var entity = await session.LoadAsync<Application>(application.Id);

        //        if(string.IsNullOrEmpty(entity.CreatedBy) && !string.IsNullOrEmpty(application.CreatedBy))
        //        {
        //            entity.CreatedBy = application.CreatedBy;
        //        }
        //        else if (string.IsNullOrEmpty(entity.CreatedBy) && application.AdminIds?.Count() == 1)
        //        {
        //            entity.CreatedBy = application.AdminIds.FirstOrDefault()?.Id;
        //        }

        //        entity.AdminIds = application.AdminIds;

        //        await session.SaveChangesAsync();
        //        return entity;
        //    }
        //}
    }
}