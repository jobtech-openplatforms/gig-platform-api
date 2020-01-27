using System.Collections.Generic;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Managers
{
    public interface IUserManager
    {
        User GetUser(UserId userId);
        Task<User> GetUserAsync(UserId userId);
        Task<User> SaveUserAsync(User user);
        Task<User> CreateAsync(User user, string password);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> Authenticate(string username, string password);
        Task UpdateAsync(User userParam, string password = null);
        Task DeleteAsync(UserId id);
    }
}