using System.Collections.Generic;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers
{
    public interface IPlatformManager
    {
        Task<IEnumerable<Platform>> GetPlatformsAsync(IAsyncDocumentSession session);
        Task<Platform> GetPlatformAsync(PlatformId platformId, IAsyncDocumentSession session);
        Task<Platform> GetPlatformByTokenAsync(PlatformToken platformToken, IAsyncDocumentSession session);
        Task<Platform> UpdatePlatformAsync(PlatformId id, PlatformRequest platformRequest, IAsyncDocumentSession session);
    }
}