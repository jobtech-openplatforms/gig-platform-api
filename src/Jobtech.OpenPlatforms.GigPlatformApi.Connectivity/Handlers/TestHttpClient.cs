using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public abstract class TestHttpClient : ITestHttpClient
    {
        protected readonly HttpClient _client;
        protected readonly ILogger<TestHttpClient> _logger;

        protected TestHttpClient(HttpClient client, ILogger<TestHttpClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<TResult> PostAsync<TRequest, TResult>(string endpoint, TRequest request, params KeyValuePair<string,string>[] headers)
        {
            try
            {
                _logger.LogInformation("POST: {endpoint} with {request}", endpoint, request);

                foreach(var header in headers)
                    _client.DefaultRequestHeaders.Add(header.Key, header.Value);

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var result = await _client.PostAsync(endpoint, content);

                result.EnsureSuccessStatusCode();
                var stringResult = await result.Content.ReadAsStringAsync();

                var accessModelResponse = JsonConvert.DeserializeObject<TResult>(stringResult);

                return accessModelResponse;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "POST: {message}", ex.Message);
                throw;
            }
        }

        public async Task<T> GetAsync<T>(string endpoint, params KeyValuePair<string, string>[] headers)
        {
            System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
            try
            {
                _logger.LogInformation("GET: {endpoint}", endpoint);

                foreach (var header in headers)
                    _client.DefaultRequestHeaders.Add(header.Key, header.Value);

                var result = await _client.GetAsync(endpoint);

                statusCode = result.StatusCode;

                result.EnsureSuccessStatusCode();
                var stringResult = await result.Content.ReadAsStringAsync();

                var accessModelResponse = JsonConvert.DeserializeObject<T>(stringResult);

                return accessModelResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET Exception: {ex} {@statusCode}", ex, statusCode);
                throw new ApiException(ex, (int)statusCode, new List<string> { ex.Message, statusCode.ToString() });
            }
        }
    }
}