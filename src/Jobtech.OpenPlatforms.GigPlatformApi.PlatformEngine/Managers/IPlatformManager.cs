using System.Collections.Generic;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;

namespace Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers
{
    public interface IPlatformManager
    {
        Task<IEnumerable<Platform>> GetPlatformsAsync();
        Task<Platform> GetPlatformAsync(PlatformId platformId);
        Task<Platform> GetPlatformByTokenAsync(PlatformToken platformToken);
        Task<Platform> UpdatePlatformAsync(PlatformId id, PlatformRequest platformRequest);
    }
}