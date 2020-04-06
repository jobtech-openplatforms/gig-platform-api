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
    public class ApplicationHttpClient : IApplicationHttpClient
    {
        private readonly HttpClient _client;
        private readonly IAuthenticationConfigService _config;
        private readonly ILogger<ApplicationHttpClient> _logger;

        public ApplicationHttpClient(HttpClient client, IAuthenticationConfigService authenticationConfigService, ILogger<ApplicationHttpClient> logger)
        {
            _client = client;
            _config = authenticationConfigService;
            _client.DefaultRequestHeaders.Add("admin-key", _config.AdminKey);
            _logger = logger;
        }

        public async Task<GetApplicationResult> Get(string applicationId)
        {
            System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
            try
            {
                _logger.LogInformation("Get application request {@id}", applicationId);

                var result = await _client.GetAsync(_config.ApiEndpointGetApplication);
                _logger.LogInformation("Sending request to {apiEndpoint}", _config.ApiEndpointGetApplication);

                if ((int)result.StatusCode < 400)
                    _logger.LogInformation("Get application status code {@statusCode}", result.StatusCode);
                else
                {
                    _logger.LogError("Get application status code {@statusCode}", result.StatusCode);
                }

                statusCode = result.StatusCode;

                result.EnsureSuccessStatusCode();
                var stringResult = await result.Content.ReadAsStringAsync();

                _logger.LogInformation("Get application result {@result}", result);

                var accessModelResponse = JsonConvert.DeserializeObject<GetApplicationResult>(stringResult);

                return accessModelResponse;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unable to get application.");
                throw new ApiException (ex, (int)statusCode, new List<string> { ex.Message, statusCode.ToString() });
            }
        }

        public async Task<CreateApplicationResult> CreateApplication(CreateApplicationModel request)
        {
            try
            {
                _logger.LogInformation("Create application request {@request}", request);

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var result = await _client.PostAsync(_config.ApiEndpointCreateApplication, content);
                _logger.LogInformation("Post {content}", content);
                _logger.LogInformation("Sending request to {apiEndpoint}", _config.ApiEndpointCreateApplication);

                if ((int)result.StatusCode < 400)
                    _logger.LogInformation("Create application status code {@statusCode}", result.StatusCode);
                else
                {
                    _logger.LogError("Create application status code {@statusCode}", result.StatusCode);
                }
                // Read the response body for debugging
                var debugResult = await result.Content.ReadAsStringAsync();
                _logger.LogError("Create application debug {@debugResult}", debugResult);


                result.EnsureSuccessStatusCode();
                var stringResult = await result.Content.ReadAsStringAsync();

                _logger.LogInformation("Create application {@result}", result);

                var accessModelResponse = JsonConvert.DeserializeObject<CreateApplicationResult>(stringResult);

                return accessModelResponse;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unable to create application from request.");
                throw;
            }
        }


        public async Task PatchApiEndpointAppSetNotificationUrl(string applicationId, string url)
            => await Patch(_config.ApiEndpointAppSetNotificationUrl, applicationId, url);
        public async Task PatchEmailVerificationUrl(string applicationId, string url)
            => await Patch(_config.ApiEndpointAppSetEmailVerificationUrl, applicationId, url);
        public async Task PatchAuthCallbackUrl(string applicationId, string url)
            => await Patch(_config.ApiEndpointAppSetAuthCallbackUrl, applicationId, url);

        public async Task Patch(string endpoint, string applicationId, string url)
        {
            try
            {
                // TODO: Make a model for this
                var request = new { applicationId, url };
                _logger.LogInformation("Patch application request {@request}", request);

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var result = await _client.PatchAsync(endpoint, content);

                _logger.LogInformation("Patch sent to {endpoint} {@content}", endpoint, content);

                if ((int)result.StatusCode < 400)
                    _logger.LogInformation("Patch application response status code {@statusCode}", result.StatusCode);
                else
                {
                    _logger.LogError("Patch response {@statusCode}", result.StatusCode);
                }
                // Read the response body for debugging
                var debugResult = await result.Content.ReadAsStringAsync();
                _logger.LogError("Patch application debug {@debugResult}", debugResult);

                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unable to patch application from request.");
                throw new ApiException("Unable to update application. Server error.", 500, new List<string> { ex.Message });
            }
        }

    }
}