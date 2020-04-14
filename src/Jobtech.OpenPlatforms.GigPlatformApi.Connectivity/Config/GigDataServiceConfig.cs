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

        public string ApiEndpointPlatformSetName { get => ApiEndpointPlatformUrlById + "/set-name"; }
        public string ApiEndpointPlatformSetWebsiteUrl { get => ApiEndpointPlatformUrlById + "/set-websiteurl"; }
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
        public string ApiEndpointAppSetDescription { get => ApiEndpointAppUrlById + "/set-description"; }
        public string ApiEndpointAppSetLogoUrl { get => ApiEndpointAppUrlById + "/set-logourl"; }
        public string ApiEndpointAppSetWebsiteUrl { get => ApiEndpointAppUrlById + "/set-websiteurl"; }
        public string ApiEndpointAppRotateSecret { get => ApiEndpointAppUrlById + "/rotate-secret"; }
        public string ApiEndpointAppActivate { get => ApiEndpointAppUrlById + "/activate"; }
        public string ApiEndpointAppDeactivate { get => ApiEndpointAppUrlById + "/deactivate"; }
    }
}
