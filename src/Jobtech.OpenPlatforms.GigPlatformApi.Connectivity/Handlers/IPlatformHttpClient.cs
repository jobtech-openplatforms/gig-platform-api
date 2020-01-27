using AF.Gig.Common.Models;
using AF.GigPlatform.Connectivity.Models;
using AF.GigPlatform.Core.Entities;
using AF.GigPlatform.Core.Entities.Api;
using System.Threading.Tasks;

namespace AF.GigPlatform.Connectivity.Handlers
{
    public interface IPlatformHttpClient
    {
        Task<PlatformDataUserUpdateResult> GetUserDataFromPlatformAsync(UserDataRequest request, string exportDataUri);
        Task<PlatformDataUserUpdateResult> RequestUserDataFromPlatformAsync(UserDataRequest request, string exportDataUri);
        Task<PlatformDataUserTestResult> TestUserDataFromPlatformAsync(UserDataRequest request, string exportDataUri);
    }
}