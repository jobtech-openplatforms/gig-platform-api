using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public interface IGigDataHttpClient
    {
        Task<PlatformViewModel> CreatePlatform(CreatePlatformModel request);
        Task<CreateApplicationResult> CreateApplication(CreateApplicationModel request);
        Task<PlatformResponse> PlatformStatus(ProjectModel request);
        Task ActivatePlatform(ProjectModel request);
        Task DeactivatePlatform(ProjectModel request);
    }
}