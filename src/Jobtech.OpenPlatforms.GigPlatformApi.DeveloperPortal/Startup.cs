using System;
using System.Linq;
using AutoMapper;
using Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Config;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Services;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Helpers;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.HttpClients;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.EventDispatcher.IoC;
using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Config;
using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Services;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.IoC;
using Jobtech.OpenPlatforms.GigPlatformApi.Store.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rebus.ServiceProvider;
using VueCliMiddleware;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthorization();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Authority = domain;
                x.Audience = Configuration["Auth0:Audience"];
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                };
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IPlatformAdminUserManager>();
                        var documentStoreHolder = context.HttpContext.RequestServices.GetRequiredService<IDocumentStoreHolder>();
                        var uniqueIdentifier = context.Principal.Identity.Name;
                        
                        var auth0Client = context.HttpContext.RequestServices.GetRequiredService<Auth0Client>();
                        var authorizationValue = context.HttpContext.Request.Headers["Authorization"].First();
                        var accessToken = authorizationValue.Substring("Bearer".Length).Trim();
                        var userInfo = await auth0Client.GetUserInfo(accessToken);

                        using var session = documentStoreHolder.Store.OpenAsyncSession();
                        var user = await userService.GetOrCreateUserAsync(uniqueIdentifier, session);
                        user.Name = userInfo.Name;
                        user.Email = userInfo.Email;
                        await session.SaveChangesAsync();
                    }
                };
            });

            services.AddCors();

            services.AddHttpClient<Auth0Client>(client => { client.BaseAddress = new Uri(domain); });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ApiExceptionFilter>();

            services.AddMvc(options =>
                                {
                                    options.Filters.Add(typeof(ApiExceptionFilter));
                                    options.Filters.Add(typeof(ValidateModelStateAttribute));
                                })
                          .AddNewtonsoftJson(options =>
                          {
                              options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                              options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                              //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                          })
                        .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            var applicationInsightsSection = Configuration.GetSection("ApplicationsInsights");
            var instrumentationKey = applicationInsightsSection["InstrumentationKey"];

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Trace);
                loggingBuilder.AddApplicationInsights(instrumentationKey);
            });
            services.AddApplicationInsightsTelemetry(instrumentationKey);

            // In production, the Vue files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            //Add functionality to inject IOptions<T>
            services.AddOptions();

            // App Settings
            services.Configure<RavenConfig>(Configuration.GetSection("Raven"));

            // Document store for Raven
            services.AddSingleton<IDocumentStoreHolder, DocumentStoreHolder>();

            services.AddSingleton<IConfiguration>(Configuration);

            // Configure auth
            services.AddSingleton<IAuthenticationConfigService, AuthenticationConfigService>();
            services.Configure<GigDataServiceConfig>(Configuration.GetSection("GigDataService"));

            // Configure file storage
            services.AddScoped<IFileStorageConfigService, FileStorageConfigService>();
            services.Configure<FileStoreConfig>(Configuration.GetSection("FileStoreConfig"));
            services.AddScoped<IFileManager, FileManager>();

            // Can't this be done smarter with .Net CORE DI? Do we need a better DI?
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IPlatformAdminUserManager, PlatformAdminUserManager>();

            services.AddEventDispatcher(Configuration);
            services.AddPlatformEngine(Configuration);
            services.AddHttpClient<IPlatformHttpClient, PlatformHttpClient>();
            services.AddHttpClient<IGigDataHttpClient, GigDataHttpClient>();

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gig Platform API", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.ApplicationServices.UseRebus();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseAuthentication();
            //app.UseMvc(); // Removed in .Net Core 3.0

            app.UseDefaultFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // TODO: Swagger config - static files are not working
            //app.UseStaticFiles();
            //app.UseSwagger(c =>
            //{
            //    c.RouteTemplate = "docs/{documentName}/docs.json";
            //});
            //app.UseSwaggerUI(c =>
            //{
            //    c.RoutePrefix = string.Empty;

            //    c.SwaggerEndpoint("/swagger/v1/docs.json", "My Gig Data API V1");
            //    c.InjectStylesheet("/swagger-ui/custom.css");
            //    c.InjectJavascript("/swagger-ui/custom.js");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                if (env.IsDevelopment())
                {
                    // initialize vue cli middleware
                    endpoints.MapToVueCliProxy("{*path}", new SpaOptions { SourcePath = "ClientApp" }, "serve", regex: "Compiled successfully");
                }
            });
        }
    }
}