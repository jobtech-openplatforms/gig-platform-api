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
            Api = gigDataServiceConfig.Value;
           
        }

        public string Token { get; }
        public string AdminKey { get; }
        public GigDataServiceConfig Api { get; }
    }
}