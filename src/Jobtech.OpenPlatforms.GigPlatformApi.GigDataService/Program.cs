using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace Jobtech.OpenPlatforms.GigPlatformApi.GigDataService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights("2a5c1c33-811e-4a36-96d7-9fa0c810d07b")
            .ConfigureLogging((hostContext, configLogging) =>
            {
                configLogging.AddConsole();
                configLogging.SetMinimumLevel(LogLevel.Trace);
                if (!hostContext.HostingEnvironment.IsDevelopment())
                {
                    configLogging.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Trace);
                    configLogging.AddApplicationInsights("2a5c1c33-811e-4a36-96d7-9fa0c810d07b");
                }
            })
                .UseStartup<Startup>();
    }
}
