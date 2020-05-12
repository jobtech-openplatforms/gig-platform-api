using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.IoC;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.IoC
{
    public static class DevPortalExtensions
    {
        public static IServiceCollection AddDevPortalDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {

            // Configure file storage
            services.AddFileStore(configuration);
            
            services.AddScoped<IProjectUpdateManager, ProjectUpdateManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IPlatformAdminUserManager, PlatformAdminUserManager>();

            return services;
        }
    }
}
