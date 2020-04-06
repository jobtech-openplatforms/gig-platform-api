using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Config;
using Microsoft.Extensions.Options;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Services
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
            ApiEndpointGetPlatform = gigDataServiceConfig.Value.ApiEndpointGetPlatform;
            ApiEndpointActivatePlatform = gigDataServiceConfig.Value.ApiEndpointActivatePlatform;
            ApiEndpointDeactivatePlatform = gigDataServiceConfig.Value.ApiEndpointDeactivatePlatform;
            ApiEndpointAppSetNotificationUrl = gigDataServiceConfig.Value.ApiEndpointAppSetNotificationUrl;
            ApiEndpointAppSetEmailVerificationUrl = gigDataServiceConfig.Value.ApiEndpointAppSetEmailVerificationUrl;
            ApiEndpointAppSetAuthCallbackUrl = gigDataServiceConfig.Value.ApiEndpointAppSetAuthCallbackUrl;
            ApiEndpointGetApplication = gigDataServiceConfig.Value.ApiEndpointGetApplication;
            ApiEndpointAppRotateSecret = gigDataServiceConfig.Value.ApiEndpointAppRotateSecret;
            ApiEndpointAppSetName = gigDataServiceConfig.Value.ApiEndpointAppSetName;
        }

        public string Token { get; }
        public string AdminKey { get; }
        public string ApiEndpointCreatePlatform { get; }
        public string ApiEndpointValidateEmail { get; }
        public string ApiEndpointCreateApplication { get; }
        public string ApiEndpointGetPlatform { get; }
        public string ApiEndpointActivatePlatform { get; }
        public string ApiEndpointDeactivatePlatform { get; }
        
        public string ApiEndpointGetApplication { get; }
        public string ApiEndpointAppRotateSecret { get; }
        public string ApiEndpointAppSetName { get; }
        public string ApiEndpointAppSetNotificationUrl { get; }
        public string ApiEndpointAppSetEmailVerificationUrl { get; }
        public string ApiEndpointAppSetAuthCallbackUrl { get;  }
    }
}