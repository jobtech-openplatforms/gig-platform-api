namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Services
{
    public interface IAuthenticationConfigService
    {
        string Token { get; }
        string AdminKey { get; }
        string ApiEndpointCreatePlatform { get; }
        string ApiEndpointValidateEmail { get; }
        string ApiEndpointCreateApplication { get; }
        string ApiEndpointActivatePlatform { get; }
        string ApiEndpointDeactivatePlatform { get; }
    }
}
