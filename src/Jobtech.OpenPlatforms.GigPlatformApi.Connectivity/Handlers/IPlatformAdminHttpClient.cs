using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    /// <summary>
    /// Communication for updating platform data
    /// </summary>
    public interface IPlatformAdminHttpClient : IGigDataHttpClient
    {
        Task<PlatformViewModel> CreatePlatform(CreatePlatformModel request);
        Task<PlatformResponse> GetPlatform(ProjectModel request);
        Task<PlatformResponse> GetPlatform(PlatformId id);
        
        Task SetName(string platformId, string name);
        Task SetDescription(string platformId, string description);
        Task SetLogoUrl(string platformId, string logoUrl);
        Task SetWebsiteUrl(string platformId, string websiteUrl);
        Task ActivatePlatform(ProjectModel request);
        Task DeactivatePlatform(ProjectModel request);

    }
}