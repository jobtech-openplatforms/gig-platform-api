using AF.Gig.Common.Models;
using AF.GigPlatform.Connectivity.Models;
using AF.GigPlatform.Core.Entities;
using System.Threading.Tasks;

namespace AF.GigPlatform.Connectivity.Handlers
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