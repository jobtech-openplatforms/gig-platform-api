using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using AF.GigPlatform.PlatformEngine.Managers;

namespace AF.GigPlatform.PlatformEngine.IoC
{
    public static class PlatformEngineServiceExtension
    {
        public static IServiceCollection AddPlatformEngine(this IServiceCollection collection,
            IConfiguration configuration)
        {

            collection.AddTransient<IConnectionManager, ConnectionManager>();
            collection.AddTransient<IConnectionUserManager, ConnectionUserManager>();
            collection.AddTransient<IPlatformManager, PlatformManager>(); // TODO: remove
            collection.AddTransient<IApplicationManager, ApplicationManager>(); // TODO: remove
            collection.AddTransient<IProjectManager, ProjectManager>();
            collection.AddTransient<IUserPlatformConnectionManager, UserPlatformConnectionManager>();

            return collection;
        }
    }
}
