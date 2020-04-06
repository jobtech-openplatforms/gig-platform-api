namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Services
{
    public interface IAuthenticationConfigService
    {
        string Token { get; }
        string AdminKey { get; }
        string ApiEndpointCreatePlatform { get; }
        string ApiEndpointGetPlatform { get; }
        string ApiEndpointValidateEmail { get; }
        string ApiEndpointActivatePlatform { get; }
        string ApiEndpointDeactivatePlatform { get; }

        string ApiEndpointGetApplication { get; }
        string ApiEndpointCreateApplication { get; }
        string ApiEndpointAppSetNotificationUrl { get; }
        string ApiEndpointAppSetEmailVerificationUrl { get; }
        string ApiEndpointAppSetAuthCallbackUrl { get; }
    }
}
