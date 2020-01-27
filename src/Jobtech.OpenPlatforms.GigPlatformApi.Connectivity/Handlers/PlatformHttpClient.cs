using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Newtonsoft.Json;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public class PlatformHttpClient : IPlatformHttpClient
    {
        private readonly HttpClient _client;

        public PlatformHttpClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<PlatformUserAccessTokenResponse> GetUserAccessTokenAsync(PlatformWebhookRequest request, string exportDataUri)
        {
            _client.BaseAddress = new Uri(exportDataUri);

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync(exportDataUri, content);

            result.EnsureSuccessStatusCode();
            var stringResult = await result.Content.ReadAsStringAsync();

            var accessModelResponse = JsonConvert.DeserializeObject<PlatformUserAccessTokenResponse>(stringResult);

            return accessModelResponse;
        }

        public async Task<PlatformUserAccessTokenResponse> GetUserAccessTokenAsync(UserUpdateRequest request, string exportDataUri)
        {
            _client.BaseAddress = new Uri(exportDataUri);

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync(exportDataUri, content);

            result.EnsureSuccessStatusCode();
            var stringResult = await result.Content.ReadAsStringAsync();

            var accessModelResponse = JsonConvert.DeserializeObject<PlatformUserAccessTokenResponse>(stringResult);

            return accessModelResponse;
        }

        public async Task<PlatformDataUserUpdateResult> GetUserDataFromPlatformAsync(UserDataRequest request, string exportDataUri)
        {
            _client.BaseAddress = new Uri(exportDataUri);

            // For GETting the user data from the external platform
            //_client.DefaultRequestHeaders.Add("myGigDataToken", request.MyGigDataToken);
            //_client.DefaultRequestHeaders.Add("username", request.Username);
            //_client.DefaultRequestHeaders.Add("requestId", request.RequestId);
            //var response = await _client.GetAsync("");

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(exportDataUri, content);

            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();

            var result =  JsonConvert.DeserializeObject<PlatformDataUserUpdateResult>(stringResult);
            result.RawData = stringResult;
            return result;
        }

        public async Task<PlatformDataUserTestResult> TestUserDataFromPlatformAsync(UserDataRequest request, string exportDataUri)
        {
            _client.BaseAddress = new Uri(exportDataUri);


            // For GETting the user data from the external platform
            //_client.DefaultRequestHeaders.Add("myGigDataToken", request.MyGigDataToken);
            //_client.DefaultRequestHeaders.Add("username", request.Username);
            //_client.DefaultRequestHeaders.Add("requestId", request.RequestId);
            //var response = await _client.GetAsync("");

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(exportDataUri, content);

            
            var stringResult = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            var result =  JsonConvert.DeserializeObject<PlatformDataUserUpdateResult>(stringResult);
            return new PlatformDataUserTestResult(result,
                new TestRequest(null, request
                    )
                , new TestResponse(
                    response.Headers.Select(item => string.Format("{0} : {1}", item.Key, string.Join(", ", item.Value))).ToArray()
                    , response.StatusCode.ToString()
                    , stringResult
                ));
        }

        public async Task<PlatformDataUserUpdateResult> RequestUserDataFromPlatformAsync(UserDataRequest request, string exportDataUri)
        {
            _client.BaseAddress = new Uri(exportDataUri);

            // For POST requesting the user data from the external platform
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("", stringContent);
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PlatformDataUserUpdateResult>(stringResult);
        }

        //public async Task<GenericResponse> CallPlatformForUserDataAsync(Platform platform)
        //{
        //    _client.BaseAddress = new Uri(platform.ExportDataUri);

        //    // For GETting the user data from the external platform
        //    _client.DefaultRequestHeaders.Add("myGigDataToken", platform.MyGigDataToken);
        //    _client.DefaultRequestHeaders.Add("username", request.Username);
        //    _client.DefaultRequestHeaders.Add("requestId", request.Username);
        //    var response = await _client.GetAsync("");
        //    response.EnsureSuccessStatusCode();
        //    var stringResult = await response.Content.ReadAsStringAsync();

        //    return JsonConvert.DeserializeObject<GenericResponse>(stringResult);
        //}
    }
}