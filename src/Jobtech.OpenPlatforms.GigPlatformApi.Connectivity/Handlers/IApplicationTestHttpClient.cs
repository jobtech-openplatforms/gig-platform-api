using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models.ApplicationModels;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using System;
using System.Threading.Tasks;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public interface IApplicationTestHttpClient
    {
        Task<ApplicationTestResponse> SendAuthResponse(Application application, PlatformId platformId, string state, string result, string openPlatformsUserId, int permissions = 1);
        Task<ApplicationTestResponse> SendGetToApplication(Uri uri);
    }
}