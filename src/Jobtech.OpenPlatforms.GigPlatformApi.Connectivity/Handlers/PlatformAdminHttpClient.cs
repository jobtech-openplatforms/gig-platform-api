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
        // private readonly HttpClient _client;
        // private readonly IAuthenticationConfigService _config;
        // private readonly ILogger<GigDataPlatformHttpClient> _logger;

        public PlatformAdminHttpClient(HttpClient client, IAuthenticationConfigService authenticationConfigService, ILogger<PlatformAdminHttpClient> logger)
        : base(client, authenticationConfigService, logger)
        {
            // _client = client;
            // _config = authenticationConfigService;
            // _client.DefaultRequestHeaders.Add("admin-key", _config.AdminKey);
            // _logger = logger;
        }

        public async Task<PlatformViewModel> CreatePlatform(CreatePlatformModel request)
            => await CreateAsync<CreatePlatformModel, PlatformViewModel>(_config.Api.ApiEndpointCreatePlatform, request, nameof(Core.Entities.Platform));
        // {
        //         _logger.LogInformation("Create platform request {@request}", request);

        //     var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        //     var result = await _client.PostAsync(_config.Api.ApiEndpointCreatePlatform, content);

        //     result.EnsureSuccessStatusCode();
        //     var stringResult = await result.Content.ReadAsStringAsync();
        //     _logger.LogInformation("Create platform response string {@stringResult}", stringResult);

        //     var accessModelResponse = JsonConvert.DeserializeObject<PlatformViewModel>(stringResult);
            
        //     _logger.LogInformation("Create platform response {@accessModelResponse}", accessModelResponse);

        //     return accessModelResponse;
        // }

        public async Task<PlatformResponse> GetPlatform(ProjectModel request)
            => await GetAsync<PlatformResponse>(
                        _config.Api.ApiEndpointGetPlatform.Replace("{platformId}", request.PlatformId),
                        request.PlatformId,
                        nameof(Core.Entities.Platform));
        // {
        //         _logger.LogInformation("Platform status request {@request}", request);

        //     // var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        //     var result = await _client.GetAsync(_config.Api.ApiEndpointGetPlatform.Replace("{platformId}", request.PlatformId));

        //     var stringResult = await result.Content.ReadAsStringAsync();
        //     try
        //     {

        //         result.EnsureSuccessStatusCode();
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError("Request error response {@stringResult}", stringResult);
        //         _logger.LogError(ex, "Request failed {@client}", _client);
        //         var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(stringResult);
        //         throw new ApiException(ex, "Unable to get platform status.", result.StatusCode, new List<string> { errorResponse.Error, ex.Message });
        //     }

        //     var response = JsonConvert.DeserializeObject<PlatformResponse>(stringResult);

        //     return response;
        // }

        public async Task ActivatePlatform(ProjectModel request)
            => await PatchAsync(_config.Api.ApiEndpointActivatePlatform.Replace("{platformId}", request.PlatformId), null);
        // {
        //         _logger.LogInformation("Activate platform request {@request}", request);

        //     var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        //     var result = await _client.PatchAsync(_config.Api.ApiEndpointActivatePlatform.Replace("{platformId}", request.PlatformId), content);

        //     var stringResult = await result.Content.ReadAsStringAsync();
        //     try
        //     {

        //         result.EnsureSuccessStatusCode();
        //     }
        //     catch (Exception ex)
        //     {

        //         var response = JsonConvert.DeserializeObject<ErrorResponse>(stringResult);
        //         throw new ApiException("Unable to activate platform.", (int)result.StatusCode, new List<string> { response.Error, ex.Message });
        //     }
        // }

        public async Task DeactivatePlatform(ProjectModel request)
            => await PatchAsync(_config.Api.ApiEndpointDeactivatePlatform.Replace("{platformId}", request.PlatformId), null);
        // {
        //         _logger.LogInformation("Deactivate platform request {@request}", request);

        //     var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        //     var result = await _client.PatchAsync(_config.Api.ApiEndpointDeactivatePlatform.Replace("{platformId}", request.PlatformId), content);

        //     var stringResult = await result.Content.ReadAsStringAsync();
        //     try
        //     {

        //         result.EnsureSuccessStatusCode();
        //     }
        //     catch (Exception ex)
        //     {

        //         var response = JsonConvert.DeserializeObject<ErrorResponse>(stringResult);
        //         throw new ApiException("Unable to deactivate platform.", (int)result.StatusCode, new List<string> { response.Error, ex.Message });
        //     }
        // }

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