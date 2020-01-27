using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Config;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Services;
using Jobtech.OpenPlatforms.GigPlatformApi.EventDispatcher.IoC;
using Jobtech.OpenPlatforms.GigPlatformApi.EventDispatcher.MessageHandlers;
using Jobtech.OpenPlatforms.GigPlatformApi.GigDataService.AuthenticationHandlers;
using Jobtech.OpenPlatforms.GigPlatformApi.GigDataService.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.IoC;
using Jobtech.OpenPlatforms.GigPlatformApi.Store.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Rebus.ServiceProvider;

namespace Jobtech.OpenPlatforms.GigPlatformApi.GigDataService
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Starting up");
            services.AddMvc(options =>
            {
                options.Filters.Add(new ApiExceptionFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // configure basic authentication
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            // Document store for Raven
            services.Configure<RavenConfig>(Configuration.GetSection("Raven"));
            services.AddSingleton<IDocumentStoreHolder, DocumentStoreHolder>();
            services.AddSingleton<IConfiguration>(Configuration);

            // Configure auth
            services.AddSingleton<IAuthenticationConfigService, AuthenticationConfigService>();
            services.Configure<GigDataServiceConfig>(Configuration.GetSection("GigDataService"));

            var applicationInsightsSection = Configuration.GetSection("ApplicationsInsights");
            var instrumentationKey = applicationInsightsSection["InstrumentationKey"];

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Trace);
                loggingBuilder.AddApplicationInsights(instrumentationKey);
            });
            services.AddApplicationInsightsTelemetry(instrumentationKey);

            // Platform engine
            services.AddPlatformEngine(Configuration);

            // Connectivity
            services.AddHttpClient<IPlatformHttpClient, PlatformHttpClient>();
            services.AddHttpClient<IGigDataHttpClient, GigDataHttpClient>();

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "GigDataService Internal API", Version = "v1" });
            //});

            // Configure and register Rebus
            services.AutoRegisterHandlersFromAssemblyOf<PlatformUserDataMessageHandler>();

            // Event dispatcher for messaging
            services.AddEventDispatcher(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //// Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();

            //// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            //// specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GigDataService Internal API V1");
            //    c.RoutePrefix = string.Empty;
            //});

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();

            app.ApplicationServices.UseRebus();
        }
    }
}