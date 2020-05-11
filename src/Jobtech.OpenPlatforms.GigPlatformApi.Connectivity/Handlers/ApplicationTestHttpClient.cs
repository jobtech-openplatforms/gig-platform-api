using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.ApplicationApi;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Extensions;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models.ApplicationModels;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public class ApplicationTestHttpClient : TestHttpClient, IApplicationTestHttpClient
    {
        public ApplicationTestHttpClient(HttpClient client, ILogger<ApplicationTestHttpClient> logger)
            : base(client, logger)
        {
        }

        public async Task<ApplicationTestResponse> SendDataTest(Application application, PlatformConnectionUpdateNotificationPayload payload)
        {

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var uri = new Uri(application.DataUpdateCallbackUrl);
            return await SendPostToApplication(uri, content);
        }

        public async Task<ApplicationTestResponse> SendAuthCallback(Application application, string requestId, string result, string openPlatformsUserId)
        {
            try
            {
                Validate(application, requestId, result, openPlatformsUserId);
            }
            catch (Exception ex)
            {
                return new ApplicationTestResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    TestedUrl = "",
                    Message = ex.Message
                };
            }
            var uri = new Uri(application.AuthCallbackUrl);
            uri = uri.AddParameter(nameof(requestId), requestId)
                        .AddParameter(nameof(result), result)
                        .AddParameter(nameof(openPlatformsUserId), openPlatformsUserId)
                        ;
            return await SendGetToApplication(uri);
        }

        public async Task<ApplicationTestResponse> SendGetToApplication(Uri uri)
        {
            var response = await _client.GetAsync(uri);
            if ((int)response.StatusCode > 399)
            {
                _logger.LogError("GET response: {@statusCode} {@uri} {@body}", response.StatusCode, uri.ToString(), await response.Content.ReadAsStringAsync());
            }
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                return new ApplicationTestResponse
                {
                    StatusCode = response.StatusCode,
                    Success = response.StatusCode < System.Net.HttpStatusCode.BadRequest,
                    TestedUrl = uri.ToString(),
                    Message = ex.Message
                };
            }
            return new ApplicationTestResponse
            {
                StatusCode = response.StatusCode,
                Success = response.StatusCode < System.Net.HttpStatusCode.BadRequest,
                TestedUrl = uri.ToString(),
                Message = "Request succeeded"
            };
        }

        public async Task<ApplicationTestResponse> SendPostToApplication(Uri uri, HttpContent content)
        {
            var response = await _client.PostAsync(uri, content);
            if ((int)response.StatusCode > 399)
            {
                _logger.LogError("POST response: {@statusCode} {@uri} {@body}", response.StatusCode, uri.ToString(), await response.Content.ReadAsStringAsync());
            }
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                return new ApplicationTestResponse
                {
                    StatusCode = response.StatusCode,
                    Success = response.StatusCode < System.Net.HttpStatusCode.BadRequest,
                    TestedUrl = uri.ToString(),
                    Message = ex.Message
                };
            }
            return new ApplicationTestResponse
            {
                StatusCode = response.StatusCode,
                Success = response.StatusCode < System.Net.HttpStatusCode.BadRequest,
                TestedUrl = uri.ToString(),
                Message = "Request succeeded"
            };
        }

        private void Validate(Application application, string requestId, string result, string openPlatformsUserId)
        {
            new Uri(application.AuthCallbackUrl);
            if (string.IsNullOrEmpty(requestId))
            {
                throw new Exception("State cannot be null.");
            }
            if (!(new List<string> { "completed", "failed", "cancelled" }).Contains(result))
            {
                throw new Exception($"Result has to be 'completed', 'cancelled' or 'failed'. (was '{result}')");
            }
            Guid.Parse(openPlatformsUserId);
        }
    }
}