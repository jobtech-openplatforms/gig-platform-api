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
                a.ApiEndpointActivatePlatform = section.GetValue<string>(nameof(GigDataServiceConfig.ApiEndpointActivatePlatform));
                a.ApiEndpointCreatePlatform = section.GetValue<string>(nameof(GigDataServiceConfig.ApiEndpointActivatePlatform));
                a.ApiEndpointDeactivatePlatform = section.GetValue<string>(nameof(GigDataServiceConfig.ApiEndpointDeactivatePlatform));
                a.ApiEndpointGetPlatform = section.GetValue<string>(nameof(GigDataServiceConfig.ApiEndpointGetPlatform));
                a.ApiEndpointValidateEmail = section.GetValue<string>(nameof(GigDataServiceConfig.ApiEndpointValidateEmail));

                a.ApiEndpointCreateApplication = section.GetValue<string>(nameof(GigDataServiceConfig.ApiEndpointCreateApplication));
                a.ApiEndpointGetApplication = section.GetValue<string>(nameof(GigDataServiceConfig.ApiEndpointGetApplication));

                a.ApiEndpointAppSetNotificationUrl = section.GetValue<string>(nameof(GigDataServiceConfig.ApiEndpointAppSetNotificationUrl));
                a.ApiEndpointAppSetEmailVerificationUrl = section.GetValue<string>(nameof(GigDataServiceConfig.ApiEndpointAppSetEmailVerificationUrl));
                a.ApiEndpointAppSetAuthCallbackUrl = section.GetValue<string>(nameof(GigDataServiceConfig.ApiEndpointAppSetAuthCallbackUrl));

                a.ApiEndpointAppSetName = section.GetValue<string>(nameof(GigDataServiceConfig.ApiEndpointAppSetName));
                a.ApiEndpointAppRotateSecret = section.GetValue<string>(nameof(GigDataServiceConfig.ApiEndpointAppRotateSecret));

            });
            collection.AddTransient<IAuthenticationConfigService, AuthenticationConfigService>();

            collection.AddHttpClient<IGigDataHttpClient, GigDataHttpClient>();
            collection.AddHttpClient<IApplicationHttpClient, ApplicationHttpClient>();

            return collection;
        }

        public static IServiceCollection AddPlatformEndpointConnectivity(this IServiceCollection collection)
        {
            collection.AddHttpClient<IPlatformHttpClient, PlatformHttpClient>();

            return collection;
        }
    }
}
