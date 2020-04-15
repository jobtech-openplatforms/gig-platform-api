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
    public abstract class GigDataHttpClient : IGigDataHttpClient
    {
        protected readonly HttpClient _client;
        protected readonly IAuthenticationConfigService _config;
        protected readonly ILogger<GigDataHttpClient> _logger;

        protected GigDataHttpClient(HttpClient client, IAuthenticationConfigService authenticationConfigService, ILogger<GigDataHttpClient> logger)
        {
            _client = client;
            _config = authenticationConfigService;
            _client.DefaultRequestHeaders.Add("admin-key", _config.AdminKey);
            _logger = logger;
        }

        public async Task<K> CreateAsync<T, K>(string endpoint, T request, string creatingType)
        {
            try
            {
                _logger.LogInformation("CREATE {@creatingType}: request {@request}", creatingType, request);

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var result = await _client.PostAsync(endpoint, content);
                _logger.LogInformation("CREATE {@creatingType}: Post {@content}", creatingType, content);
                _logger.LogInformation("CREATE {@creatingType}: Sending request to {apiEndpoint}", creatingType, endpoint);

                if ((int)result.StatusCode < 400)
                    _logger.LogInformation("CREATE {@creatingType}: status code {@statusCode}", creatingType, result.StatusCode);
                else
                {
                    _logger.LogError("CREATE {@creatingType}: status code {@statusCode}", creatingType, result.StatusCode);
                    // Read the response body for debugging
                    var debugResult = await result.Content.ReadAsStringAsync();
                    _logger.LogDebug("CREATE {@creatingType}: debug {@debugResult}", creatingType, debugResult);
                }


                result.EnsureSuccessStatusCode();
                var stringResult = await result.Content.ReadAsStringAsync();

                _logger.LogInformation("CREATE {@creatingType}: {@result}", creatingType, result);

                var accessModelResponse = JsonConvert.DeserializeObject<K>(stringResult);

                return accessModelResponse;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "CREATE {@creatingType}: Unable to create from request.", creatingType);
                throw;
            }
        }

        public async Task<T> GetAsync<T>(string endpoint, string id, string gettingType)
        {
            System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
            try
            {
                _logger.LogInformation("GET {@gettingType}: request {@id}", gettingType, id);
                // _logger.LogDebug("GET {@gettingType}: config {@config}", gettingType, _config);
                _logger.LogInformation("GET {@gettingType}: request url {@apiEndpoint}", gettingType, endpoint);

                var result = await _client.GetAsync(endpoint);
                _logger.LogInformation("GET {@gettingType}: Sent request", gettingType);

                if ((int)result.StatusCode < 400)
                    _logger.LogInformation("GET {@gettingType}: status code {@statusCode}", gettingType, result.StatusCode);
                else
                {
                    _logger.LogError("GET {@gettingType}: status code {@statusCode}", gettingType, result.StatusCode);
                }

                statusCode = result.StatusCode;

                result.EnsureSuccessStatusCode();
                var stringResult = await result.Content.ReadAsStringAsync();

                _logger.LogInformation("GET {@gettingType}: result {@result}", gettingType, result);

                var accessModelResponse = JsonConvert.DeserializeObject<T>(stringResult);

                return accessModelResponse;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "GET {@gettingType}: FAILED - Unable to GET.", gettingType);
                _logger.LogCritical(ex, "GET Exception: {ex}", ex);
                throw new ApiException(ex, (int)statusCode, new List<string> { ex.Message, statusCode.ToString() });
            }
        }

        public async Task PatchAsync(string endpoint, object request)
        {
            try
            {
                // TODO: Make a model for this
                _logger.LogInformation("Patch [1] request {@request}", request);

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var result = await _client.PatchAsync(endpoint, content);

                _logger.LogInformation("Patch [2] sent to {@endpoint}", endpoint);

                if ((int)result.StatusCode < 400)
                    _logger.LogInformation("Patch [3] response status code {@statusCode}", result.StatusCode);
                else
                {
                    _logger.LogError("Patch [3] response {@statusCode}", result.StatusCode);
                    // Read the response body for debugging
                    var debugResult = await result.Content.ReadAsStringAsync();
                    _logger.LogDebug("Patch [4] debug {@debugResult}", debugResult);
                }

                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unable to patch as requested. {endpoint}", endpoint);
                throw new ApiException("Unable to update. Server error.", 500, new List<string> { ex.Message });
            }
        }

    }
}