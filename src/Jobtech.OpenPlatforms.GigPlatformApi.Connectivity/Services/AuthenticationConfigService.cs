using AF.Gig.WebApi.Services;
using AF.GigPlatform.Connectivity.Config;
using Microsoft.Extensions.Options;

namespace AF.GigPlatform.Connectivity.Services
{
    public class AuthenticationConfigService : IAuthenticationConfigService
    {
        public AuthenticationConfigService(IOptions<GigDataServiceConfig> gigDataServiceConfig)
        {
            Token = gigDataServiceConfig.Value.Token;
            AdminKey = gigDataServiceConfig.Value.AdminKey;
            ApiEndpointCreatePlatform = gigDataServiceConfig.Value.ApiEndpointCreatePlatform;
            ApiEndpointValidateEmail = gigDataServiceConfig.Value.ApiEndpointValidateEmail;
            ApiEndpointCreateApplication = gigDataServiceConfig.Value.ApiEndpointCreateApplication;
            ApiEndpointPlatformStatus = gigDataServiceConfig.Value.ApiEndpointPlatformStatus;
            ApiEndpointActivatePlatform = gigDataServiceConfig.Value.ApiEndpointActivatePlatform;
            ApiEndpointDeactivatePlatform = gigDataServiceConfig.Value.ApiEndpointDeactivatePlatform;
        }

        public string Token { get; }
        public string AdminKey { get; }
        public string ApiEndpointCreatePlatform { get; }
        public string ApiEndpointValidateEmail { get; }
        public string ApiEndpointCreateApplication { get; }
        public string ApiEndpointPlatformStatus { get; }
        public string ApiEndpointActivatePlatform { get; }
        public string ApiEndpointDeactivatePlatform { get; }
    }
}