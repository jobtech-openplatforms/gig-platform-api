using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public interface IPlatformHttpClient
    {
        Task<PlatformDataUserUpdateResult> GetUserDataFromPlatformAsync(UserDataRequest request, string exportDataUri);
        Task<PlatformDataUserTestResult> TestUserDataFromPlatformAsync(UserDataRequest request, string exportDataUri);
    }
}