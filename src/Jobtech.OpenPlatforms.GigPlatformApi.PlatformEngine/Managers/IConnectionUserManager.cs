using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities.Api;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;

namespace Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers
{
    public interface IConnectionUserManager
    {
        Task<PlatformUser> GetAsync(ConnectionId connectionId);
        Task SaveAsync(PlatformUser user, Connection connection);
        Task<User> GetUserAsync(UserId userId);
        Task<User> GetUserAsync(UserName username);
    }
}