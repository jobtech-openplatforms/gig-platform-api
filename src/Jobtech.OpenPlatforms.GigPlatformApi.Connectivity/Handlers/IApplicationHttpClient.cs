using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService;


namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public interface IApplicationHttpClient : IGigDataHttpClient
    {
        Task<CreateApplicationResult> CreateApplication(CreateApplicationModel request);
        Task<GetApplicationResult> Get(string id);
        Task PatchApiEndpointAppSetNotificationUrl(string applicationId, string url);
        Task PatchAuthCallbackUrl(string applicationId, string url);
        Task PatchEmailVerificationUrl(string applicationId, string url);
        Task PatchApiEndpointAppSetName(string applicationId, string name);
        Task PatchApiEndpointAppSetDescription(string applicationId, string description);
        Task PatchApiEndpointAppSetLogoUrl(string applicationId, string logoUrl);
        Task PatchApiEndpointAppSetWebsiteUrl(string applicationId, string websiteUrl);
        Task ActivateApplication(string applicationId);
        Task DeactivateApplication(string applicationId);
    }
}