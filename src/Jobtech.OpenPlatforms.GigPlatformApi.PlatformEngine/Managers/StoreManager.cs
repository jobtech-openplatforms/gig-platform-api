using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers
{
    public abstract class StoreManager<T> : IStoreManager<T>
    {
        protected readonly IDocumentStore _documentStore;
        protected readonly ILogger<StoreManager<T>> _logger;

        public StoreManager(IDocumentStore documentStore, ILogger<StoreManager<T>> logger)
        {
            _documentStore = documentStore;
            _logger = logger;
        }

        public async Task<T> Create(T entity)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(entity);
                await session.SaveChangesAsync();
                return entity;
            }
        }

        public async Task Delete(StringIdentity<T> id)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var entity = await session.LoadAsync<T>(id.Value);
                session.Delete(entity);
                await session.SaveChangesAsync();
            }
        }

        public async Task<T> Get(StringIdentity<T> id)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<T>(id.Value);
            }
        }

        public async Task<T> Get(StringIdentity<T> id, IAsyncDocumentSession session)
        {
            return await session.LoadAsync<T>(id.Value);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.Query<T>().ToListAsync();
            }
        }
    }
}