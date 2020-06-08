using System.Collections.Generic;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers
{
    public interface IStoreManager<T>
    {
        Task<IEnumerable<T>> GetAll(IAsyncDocumentSession session);
        Task<T> Get(StringIdentity<T> id, IAsyncDocumentSession session);
        Task<T> Create(T entity, IAsyncDocumentSession session);
        Task Delete(StringIdentity<T> id, IAsyncDocumentSession session);
    }
}
