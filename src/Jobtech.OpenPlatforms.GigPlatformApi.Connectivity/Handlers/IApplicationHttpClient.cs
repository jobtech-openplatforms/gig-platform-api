using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService;


namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public interface IApplicationHttpClient
    {
        Task<CreateApplicationResult> CreateApplication(CreateApplicationModel request);
        Task Patch(string endpoint, string url);
        Task PatchApiEndpointAppSetNotificationUrl(string applicationId, string url);
        Task PatchAuthCallbackUrl(string applicationId, string url);
        Task PatchEmailVerificationUrl(string applicationId, string url);
        Task<GetApplicationResult> Get(string id);
    }
}