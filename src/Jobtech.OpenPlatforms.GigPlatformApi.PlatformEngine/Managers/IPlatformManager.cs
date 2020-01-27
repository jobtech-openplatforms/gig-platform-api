using AF.GigPlatform.Core.Entities;
using AF.GigPlatform.Connectivity.Models;
using AF.GigPlatform.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AF.GigPlatform.PlatformEngine.Managers
{
    public interface IPlatformManager
    {
        Task<IEnumerable<Platform>> GetPlatformsAsync();
        Task<Platform> GetPlatformAsync(PlatformId platformId);
        Task<Platform> GetPlatformByTokenAsync(PlatformToken platformToken);
        Task<Platform> UpdatePlatformAsync(PlatformId id, PlatformRequest platformRequest);
    }
}