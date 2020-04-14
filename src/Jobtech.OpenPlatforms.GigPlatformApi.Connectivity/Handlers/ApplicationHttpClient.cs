using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Services;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public class ApplicationHttpClient : GigDataHttpClient, IApplicationHttpClient
    {

        public ApplicationHttpClient(HttpClient client, IAuthenticationConfigService authenticationConfigService, ILogger<ApplicationHttpClient> logger)
            : base(client, authenticationConfigService, logger)
        {
        }

        public async Task<GetApplicationResult> Get(string applicationId)
            => await GetAsync<GetApplicationResult>(_config.Api.ApiEndpointGetApplication.Replace("{applicationId}", applicationId), applicationId, nameof(Application));
       

        public async Task<CreateApplicationResult> CreateApplication(CreateApplicationModel request)
            => await CreateAsync<CreateApplicationModel, CreateApplicationResult>(_config.Api.ApiEndpointCreateApplication, request, nameof(Application));

        public async Task SetName(string applicationId, string name)
            => await PatchAsync(_config.Api.ApiEndpointAppSetName.Replace("{applicationId}", applicationId), new { name });
        public async Task SetDescription(string applicationId, string description)
            => await PatchAsync(_config.Api.ApiEndpointAppSetDescription.Replace("{applicationId}", applicationId), new { description });
        public async Task SetLogoUrl(string applicationId, string logoUrl)
            => await PatchAsync(_config.Api.ApiEndpointAppSetLogoUrl.Replace("{applicationId}", applicationId), new { logoUrl });
        public async Task SetWebsiteUrl(string applicationId, string websiteUrl)
            => await PatchAsync(_config.Api.ApiEndpointAppSetWebsiteUrl.Replace("{applicationId}", applicationId), new { websiteUrl });

        public async Task PatchApiEndpointAppSetNotificationUrl(string applicationId, string url)
            => await PatchAsync(_config.Api.ApiEndpointAppSetNotificationUrl.Replace("{applicationId}", applicationId), new { url });
        public async Task PatchEmailVerificationUrl(string applicationId, string url)
            => await PatchAsync(_config.Api.ApiEndpointAppSetEmailVerificationUrl.Replace("{applicationId}", applicationId), new { url });
        public async Task PatchAuthCallbackUrl(string applicationId, string url)
            => await PatchAsync(_config.Api.ApiEndpointAppSetAuthCallbackUrl.Replace("{applicationId}", applicationId), new { url });

            
        public async Task ActivateApplication(string applicationId)
            => await PatchAsync(_config.Api.ApiEndpointAppActivate.Replace("{applicationId}", applicationId), null);
        public async Task DeactivateApplication(string applicationId)
            => await PatchAsync(_config.Api.ApiEndpointAppDeactivate.Replace("{applicationId}", applicationId), null);


    }
}