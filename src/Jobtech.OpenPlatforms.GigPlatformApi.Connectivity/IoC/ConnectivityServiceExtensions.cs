using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Config;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.IoC
{
    public static class ConnectivityServiceExtensions
    {
        public static IServiceCollection AddGigDataApiConnectivity(this IServiceCollection collection,
            IConfiguration configuration)
        {
            collection.Configure<GigDataServiceConfig>(a =>
            {
                var section = configuration.GetSection("GigDataService");
                a.AdminKey = section.GetValue<string>(nameof(GigDataServiceConfig.AdminKey));
                a.ApiBaseUrl = section.GetValue<string>(nameof(GigDataServiceConfig.ApiBaseUrl));
            });
            collection.AddTransient<IAuthenticationConfigService, AuthenticationConfigService>();

            collection.AddHttpClient<IGigDataHttpClient, GigDataHttpClient>();
            collection.AddHttpClient<IPlatformAdminHttpClient, PlatformAdminHttpClient>();
            collection.AddHttpClient<IApplicationHttpClient, ApplicationHttpClient>();
            collection.AddHttpClient<IApplicationTestHttpClient, ApplicationTestHttpClient>();

            return collection;
        }

        public static IServiceCollection AddPlatformEndpointConnectivity(this IServiceCollection collection)
        {
            collection.AddHttpClient<IPlatformHttpClient, PlatformHttpClient>();

            return collection;
        }
    }
}
