using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AF.Gig.Common.Models;
using AF.Gig.WebApi.Services;
using AF.GigPlatform.Connectivity.Models;
using AF.GigPlatform.Core.Exceptions;
using Newtonsoft.Json;

namespace AF.GigPlatform.Connectivity.Handlers
{
    public class GigDataHttpClient : IGigDataHttpClient
    {
        private readonly HttpClient _client;
        private readonly IAuthenticationConfigService _config;

        public GigDataHttpClient(HttpClient client, IAuthenticationConfigService authenticationConfigService)
        {
            _client = client;
            _config = authenticationConfigService;
            _client.DefaultRequestHeaders.Add("admin-key", _config.AdminKey);
        }

        public async Task<CreateApplicationResult> CreateApplication(CreateApplicationModel request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync(_config.ApiEndpointCreateApplication, content);

            result.EnsureSuccessStatusCode();
            var stringResult = await result.Content.ReadAsStringAsync();

            var accessModelResponse = JsonConvert.DeserializeObject<CreateApplicationResult>(stringResult);

            return accessModelResponse;
        }

        public async Task<PlatformViewModel> CreatePlatform(CreatePlatformModel request)
        {

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync(_config.ApiEndpointCreatePlatform, content);

            result.EnsureSuccessStatusCode();
            var stringResult = await result.Content.ReadAsStringAsync();

            var accessModelResponse = JsonConvert.DeserializeObject<PlatformViewModel>(stringResult);

            return accessModelResponse;
        }

        public async Task<PlatformResponse> PlatformStatus(ProjectModel request)
        {

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var result = await _client.PatchAsync(_config.ApiEndpointActivatePlatform.Replace("{platformId}", request.PlatformId), content);

            var stringResult = await result.Content.ReadAsStringAsync();
            try
            {

                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {

                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(stringResult);
                throw new ApiException("Unable to get platform status.", (int)result.StatusCode, new List<string> { errorResponse.Error, ex.Message });
            }

            var response = JsonConvert.DeserializeObject<PlatformResponse>(stringResult);

            return response;
        }

        public async Task ActivatePlatform(ProjectModel request)
        {

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var result = await _client.PatchAsync(_config.ApiEndpointActivatePlatform.Replace("{platformId}", request.PlatformId), content);

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

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var result = await _client.PatchAsync(_config.ApiEndpointDeactivatePlatform.Replace("{platformId}", request.PlatformId), content);

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