using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Config;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Config;
using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Services;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.IoC
{
    public static class DevPortalExtensions
    {
        public static IServiceCollection AddDevPortalDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            
            // Configure file storage
            services.AddScoped<IFileStorageConfigService, FileStorageConfigService>();
            services.Configure<FileStoreConfig>(configuration.GetSection("FileStoreConfig"));
            services.AddScoped<IFileManager, FileManager>();
            
            services.AddScoped<IProjectUpdateManager, ProjectUpdateManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IPlatformAdminUserManager, PlatformAdminUserManager>();

            return services;
        }
    }
}
