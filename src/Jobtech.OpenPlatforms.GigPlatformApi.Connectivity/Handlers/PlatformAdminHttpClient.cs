using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Services;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public class PlatformAdminHttpClient : GigDataHttpClient, IPlatformAdminHttpClient
    {

        public PlatformAdminHttpClient(HttpClient client, IAuthenticationConfigService authenticationConfigService, ILogger<PlatformAdminHttpClient> logger)
        : base(client, authenticationConfigService, logger)
        {
        }

        public async Task<PlatformViewModel> CreatePlatform(CreatePlatformModel request)
            => await CreateAsync<CreatePlatformModel, PlatformViewModel>(_config.Api.ApiEndpointCreatePlatform, request, nameof(Core.Entities.Platform));


        public async Task<PlatformResponse> GetPlatform(ProjectModel request)
            => await GetAsync<PlatformResponse>(
                        _config.Api.ApiEndpointGetPlatform.Replace("{platformId}", request.PlatformId),
                        request.PlatformId,
                        nameof(Core.Entities.Platform));

        public async Task ActivatePlatform(ProjectModel request)
            => await PatchAsync(_config.Api.ApiEndpointActivatePlatform.Replace("{platformId}", request.PlatformId), null);
       

        public async Task DeactivatePlatform(ProjectModel request)
            => await PatchAsync(_config.Api.ApiEndpointDeactivatePlatform.Replace("{platformId}", request.PlatformId), null);
       

        public async Task SetName(string platformId, string name)
            => await PatchAsync(_config.Api.ApiEndpointPlatformSetName.Replace("{platformId}", platformId), new { name });


        public async Task SetDescription(string platformId, string description)
            => await PatchAsync(_config.Api.ApiEndpointPlatformSetDescription.Replace("{platformId}", platformId), new { description });

        public async Task SetLogoUrl(string platformId, string logoUrl)
            => await PatchAsync(_config.Api.ApiEndpointPlatformSetLogoUrl.Replace("{platformId}", platformId), new { logoUrl });

        public async Task SetWebsiteUrl(string platformId, string websiteUrl)
            => await PatchAsync(_config.Api.ApiEndpointPlatformSetWebsiteUrl.Replace("{platformId}", platformId), new { websiteUrl });

    }
}