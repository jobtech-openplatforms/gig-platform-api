using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Config;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Services
{
    public interface IAuthenticationConfigService
    {
        string Token { get; }
        string AdminKey { get; }
        GigDataServiceConfig Api { get; }
    }
}
