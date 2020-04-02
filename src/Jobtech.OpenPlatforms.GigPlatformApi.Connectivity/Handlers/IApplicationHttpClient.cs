using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public interface IApplicationHttpClient
    {
        Task<CreateApplicationResult> CreateApplication(CreateApplicationModel request);
    }
}