using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.IoC;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.HttpClients;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.EventDispatcher.IoC;
using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Config;
using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Services;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.IoC;
using Jobtech.OpenPlatforms.GigPlatformApi.Store.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client.Documents;
using Serilog;
using Serilog.Formatting.Elasticsearch;

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
            var formatElastic = Configuration.GetValue("FormatLogsInElasticFormat", false);

            // Logger configuration
            var logConf = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration);

            if (formatElastic)
            {
                var logFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true);
                logConf.WriteTo.Console(logFormatter);
            }
            else
            {
                logConf.WriteTo.Console();
            }

            Log.Logger = logConf.CreateLogger();


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
                        var documentStore = context.HttpContext.RequestServices.GetRequiredService<IDocumentStore>();
                        var uniqueIdentifier = context.Principal.Identity.Name;

                        var auth0Client = context.HttpContext.RequestServices.GetRequiredService<Auth0Client>();
                        var authorizationValue = context.HttpContext.Request.Headers["Authorization"].First();
                        var accessToken = authorizationValue.Substring("Bearer".Length).Trim();
                        var userInfo = await auth0Client.GetUserInfo(accessToken);

                        using var session = documentStore.OpenAsyncSession();
                        var user = await userService.GetOrCreateUserAsync(uniqueIdentifier, session);
                        user.Name = userInfo.Name;
                        user.Email = userInfo.Email;
                        await session.SaveChangesAsync();
                    }
                };
            });

            services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll",
                        builder =>
                        {
                            builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                        });
                });

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
                          });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GigPlatform Client Api (internal)",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Email = "calle@roombler.com",
                        Name = "Calle Hunnefalk"
                    }
                });
                c.DescribeAllParametersInCamelCase();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //Add functionality to inject IOptions<T>
            services.AddOptions();

            // RavenDb
            services.AddRavenDb(Configuration);


            // Configure file storage
            services.AddScoped<IFileStorageConfigService, FileStorageConfigService>();
            services.Configure<FileStoreConfig>(Configuration.GetSection("FileStoreConfig"));
            services.AddScoped<IFileManager, FileManager>();

            // Can't this be done smarter with .Net CORE DI? Do we need a better DI?
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IPlatformAdminUserManager, PlatformAdminUserManager>();

            services.AddEventDispatcher(Configuration);
            services.AddPlatformEngine(Configuration);

            services.AddPlatformEndpointConnectivity();
            services.AddGigDataApiConnectivity(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging();

            app.UseRouting();

            // global cors policy
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();
    
            app.UseSerilogRequestLogging();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GigDataService Internal API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}