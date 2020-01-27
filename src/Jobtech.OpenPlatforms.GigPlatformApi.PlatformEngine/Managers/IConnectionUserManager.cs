using AF.GigPlatform.Core.Entities;
using AF.GigPlatform.Core.Entities.Api;
using AF.GigPlatform.Core.ValueObjects;
using System.Threading.Tasks;

namespace AF.GigPlatform.PlatformEngine.Managers
{
    public interface IConnectionUserManager
    {
        Task<PlatformUser> GetAsync(ConnectionId connectionId);
        Task SaveAsync(PlatformUser user, Connection connection);
        Task<User> GetUserAsync(UserId userId);
        Task<User> GetUserAsync(UserName username);
    }
}