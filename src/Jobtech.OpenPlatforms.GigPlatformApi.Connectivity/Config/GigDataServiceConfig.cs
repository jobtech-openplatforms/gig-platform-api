namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Config
{
    public class GigDataServiceConfig
    {

        public string Token { get; set; }
        public string AdminKey { get; set; }
        public string ApiEndpointCreatePlatform { get; set; }
        public string ApiEndpointGetPlatform { get; set; }
        public string ApiEndpointValidateEmail { get; set; }
        public string ApiEndpointActivatePlatform { get; set; }
        public string ApiEndpointDeactivatePlatform { get; set; }
        public string ApiEndpointCreateApplication { get; set; }
        public string ApiEndpointGetApplication { get; set; }
        public string ApiEndpointAppSetNotificationUrl { get; set; }
        public string ApiEndpointAppSetEmailVerificationUrl { get; set; }
        public string ApiEndpointAppSetAuthCallbackUrl { get; set; }
        public string ApiEndpointAppSetName { get; set; }
        public string ApiEndpointAppRotateSecret { get; set; }
    }
}
