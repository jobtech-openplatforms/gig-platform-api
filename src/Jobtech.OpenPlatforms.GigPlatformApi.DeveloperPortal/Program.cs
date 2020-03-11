using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();

            host.Run();
        }

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.ConfigureAppConfiguration((context, builder) =>
            {
                if (context.HostingEnvironment.IsDevelopment())
                {
                    builder.SetBasePath(context.HostingEnvironment.ContentRootPath)
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false) //load base settings
                      .AddJsonFile("appsettings.local.json", optional: true,
                        reloadOnChange: false) //load local settings
                      .AddJsonFile($"appsettings.local.{context.HostingEnvironment.EnvironmentName}.json", optional: true,
                        reloadOnChange: false) //load environment local settings
                      .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                        optional: true) //load environment settings
                      .AddEnvironmentVariables();
                }
                else
                {
                    builder.SetBasePath(context.HostingEnvironment.ContentRootPath)
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false) //load base settings
                      .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                        optional: true) //load environment settings
                      .AddEnvironmentVariables();
                }
            });
            webBuilder.UseStartup<Startup>();
        });
    }
}
