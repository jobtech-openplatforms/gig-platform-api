namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Config
{
    public class GigDataServiceConfig
    {

        public string Token { get; set; }
        public string AdminKey { get; set; }
        public string ApiBaseUrl { get; set; }
        public string ApiEndpointPlatformBase { get => ApiBaseUrl + "/api/Platform/admin"; }
        public string ApiEndpointPlatformUrlById { get => ApiEndpointPlatformBase + "/{platformId}"; }
        public string ApiEndpointCreatePlatform { get => ApiEndpointPlatformBase; }
        public string ApiEndpointGetPlatform { get => ApiEndpointPlatformUrlById; }
        public string ApiEndpointActivatePlatform { get => ApiEndpointPlatformUrlById + "/activate"; }
        public string ApiEndpointDeactivatePlatform { get => ApiEndpointPlatformUrlById + "/deactivate"; }

        public string ApiEndpointPlatformSetLogoUrl { get => ApiEndpointPlatformUrlById + "/set-logourl"; }
        public string ApiEndpointPlatformSetDescription { get => ApiEndpointPlatformUrlById + "/set-description"; }


        public string ApiEndpointAppBase { get => ApiBaseUrl + "/api/App/admin"; }
        public string ApiEndpointAppUrlById { get => ApiEndpointAppBase + "/{applicationId}"; }

        public string ApiEndpointCreateApplication { get => ApiEndpointAppBase; }
        public string ApiEndpointGetApplication { get => ApiEndpointAppUrlById; }
        public string ApiEndpointAppSetNotificationUrl { get => ApiEndpointAppUrlById + "/set-notification-endpoint-url"; }
        public string ApiEndpointAppSetEmailVerificationUrl { get => ApiEndpointAppUrlById + "/set-email-verification-notification-endpoint-url"; }
        public string ApiEndpointAppSetAuthCallbackUrl { get => ApiEndpointAppUrlById + "/set-auth-callback-url"; }
        public string ApiEndpointAppSetName { get => ApiEndpointAppUrlById + "/set-name"; }
        public string ApiEndpointAppRotateSecret { get => ApiEndpointAppUrlById + "/rotate-secret"; }
    }
}
