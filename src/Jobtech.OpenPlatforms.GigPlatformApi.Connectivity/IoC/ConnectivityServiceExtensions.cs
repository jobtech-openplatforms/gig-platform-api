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
                a.AdminKey = section.GetValue<string>("AdminKey");
                a.ApiEndpointActivatePlatform = section.GetValue<string>("ApiEndpointActivatePlatform");
                a.ApiEndpointCreateApplication = section.GetValue<string>("ApiEndpointCreatePlatform");
                a.ApiEndpointCreatePlatform = section.GetValue<string>("ApiEndpointActivatePlatform");
                a.ApiEndpointDeactivatePlatform = section.GetValue<string>("ApiEndpointDeactivatePlatform");
                a.ApiEndpointPlatformStatus = section.GetValue<string>("ApiEndpointPlatformStatus");
                a.ApiEndpointValidateEmail = section.GetValue<string>("ApiEndpointValidateEmail");

            });
            collection.AddTransient<IAuthenticationConfigService, AuthenticationConfigService>();

            collection.AddHttpClient<IGigDataHttpClient, GigDataHttpClient>();

            return collection;
        }

        public static IServiceCollection AddPlatformEndpointConnectivity(this IServiceCollection collection)
        {
            collection.AddHttpClient<IPlatformHttpClient, PlatformHttpClient>();

            return collection;
        }
    }
}
