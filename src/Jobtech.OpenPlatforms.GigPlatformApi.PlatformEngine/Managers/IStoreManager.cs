using System.Collections.Generic;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers
{
    public interface IStoreManager<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(StringIdentity<T> id);
        Task<T> Get(StringIdentity<T> id, IAsyncDocumentSession session);
        Task<T> Create(T entity);
        Task Delete(StringIdentity<T> id);
    }
}
