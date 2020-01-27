using AF.GigPlatform.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AF.GigPlatform.PlatformEngine.Managers
{
    public interface IStoreManager<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(StringIdentity<T> id);
        Task<T> Create(T entity);
        Task Delete(StringIdentity<T> id);
    }
}
