using System.Collections.Generic;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public interface ITestHttpClient
    {
        Task<TResult> PostAsync<TRequest, TResult>(string endpoint, TRequest request, params KeyValuePair<string, string>[] headers);
        Task<T> GetAsync<T>(string endpoint, params KeyValuePair<string, string>[] headers);

    }
}