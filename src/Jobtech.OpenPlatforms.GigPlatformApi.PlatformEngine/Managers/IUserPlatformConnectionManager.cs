using AF.GigPlatform.Core.Entities;
using AF.GigPlatform.Core.ValueObjects;
using System.Threading.Tasks;

namespace AF.GigPlatform.PlatformEngine.Managers
{
    public interface IUserPlatformConnectionManager
    {
        Task<UserPlatformConnection> GetAsync(PlatformToken platformToken, UserName username);
    }
}