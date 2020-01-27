using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.HttpClients
{
    public class Auth0Client
    {

        public HttpClient Client { get; }

        public Auth0Client(HttpClient client)
        {
            Client = client;
        }

        public async Task<Auth0UserInfoViewModel> GetUserInfo(string accessToken)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var userInfoStr = await Client.GetStringAsync("userinfo");
            return JsonConvert.DeserializeObject<Auth0UserInfoViewModel>(userInfoStr);
        }

    }

    public class Auth0UserInfoViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
