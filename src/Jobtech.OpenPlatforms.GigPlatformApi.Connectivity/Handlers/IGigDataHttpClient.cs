using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public interface IGigDataHttpClient
    {
        Task<TResult> CreateAsync<TRequest, TResult>(string endpoint, TRequest request, string creatingType);
        Task<T> GetAsync<T>(string endpoint, string id, string gettingType);
        Task PatchAsync(string endpoint, object request);

    }
}