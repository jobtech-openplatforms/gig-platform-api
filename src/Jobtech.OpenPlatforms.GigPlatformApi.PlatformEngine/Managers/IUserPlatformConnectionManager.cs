using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;

namespace Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers
{
    public interface IUserPlatformConnectionManager
    {
        Task<UserPlatformConnection> GetAsync(PlatformToken platformToken, UserName username);
    }
}