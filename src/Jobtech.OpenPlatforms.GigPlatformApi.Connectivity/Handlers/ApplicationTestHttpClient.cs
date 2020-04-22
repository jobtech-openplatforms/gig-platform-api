using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public class ApplicationTestHttpClient : IApplicationTestHttpClient
    {
        private readonly HttpClient _client;

        public ApplicationTestHttpClient(HttpClient client)
        {
            _client = client;
        }

        public Task SendCallbackSuccess(Uri authCallbackUrl)
        {
            throw new NotImplementedException();
        }
    }
}