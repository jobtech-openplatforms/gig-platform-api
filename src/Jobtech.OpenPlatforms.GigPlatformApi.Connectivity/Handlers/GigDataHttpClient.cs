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
    public class GigDataHttpClient : IGigDataHttpClient
    {
        private readonly HttpClient _client;
        private readonly IAuthenticationConfigService _config;
        private readonly ILogger<GigDataHttpClient> _logger;

        public GigDataHttpClient(HttpClient client, IAuthenticationConfigService authenticationConfigService, ILogger<GigDataHttpClient> logger)
        {
            _client = client;
            _config = authenticationConfigService;
            _client.DefaultRequestHeaders.Add("admin-key", _config.AdminKey);
            _logger = logger;
        }

        public async Task<PlatformViewModel> CreatePlatform(CreatePlatformModel request)
        {
                _logger.LogInformation("Create platform request {@request}", request);

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync(_config.Api.ApiEndpointCreatePlatform, content);

            result.EnsureSuccessStatusCode();
            var stringResult = await result.Content.ReadAsStringAsync();

            var accessModelResponse = JsonConvert.DeserializeObject<PlatformViewModel>(stringResult);

            return accessModelResponse;
        }

        public async Task<PlatformResponse> GetPlatform(ProjectModel request)
        {
                _logger.LogInformation("Platform status request {@request}", request);

            // var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var result = await _client.GetAsync(_config.Api.ApiEndpointGetPlatform.Replace("{platformId}", request.PlatformId));

            var stringResult = await result.Content.ReadAsStringAsync();
            try
            {

                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request failed {client}", _client);
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(stringResult);
                throw new ApiException("Unable to get platform status.", (int)result.StatusCode, new List<string> { errorResponse.Error, ex.Message });
            }

            var response = JsonConvert.DeserializeObject<PlatformResponse>(stringResult);

            return response;
        }

        public async Task ActivatePlatform(ProjectModel request)
        {
                _logger.LogInformation("Activate platform request {@request}", request);

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var result = await _client.PatchAsync(_config.Api.ApiEndpointActivatePlatform.Replace("{platformId}", request.PlatformId), content);

            var stringResult = await result.Content.ReadAsStringAsync();
            try
            {

                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {

                var response = JsonConvert.DeserializeObject<ErrorResponse>(stringResult);
                throw new ApiException("Unable to activate platform.", (int)result.StatusCode, new List<string> { response.Error, ex.Message });
            }
        }

        public async Task DeactivatePlatform(ProjectModel request)
        {
                _logger.LogInformation("Deactivate platform request {@request}", request);

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var result = await _client.PatchAsync(_config.Api.ApiEndpointDeactivatePlatform.Replace("{platformId}", request.PlatformId), content);

            var stringResult = await result.Content.ReadAsStringAsync();
            try
            {

                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {

                var response = JsonConvert.DeserializeObject<ErrorResponse>(stringResult);
                throw new ApiException("Unable to deactivate platform.", (int)result.StatusCode, new List<string> { response.Error, ex.Message });
            }
        }
    }
}