using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.ApplicationApi;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models.ApplicationModels;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using System;
using System.Threading.Tasks;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public interface IApplicationTestHttpClient
    {
        Task<ApplicationTestResponse> SendDataTest(Application application, PlatformConnectionUpdateNotificationPayload payload);
        Task<ApplicationTestResponse> SendAuthCallback(Application application, string requestId, string result, string openPlatformsUserId);
        Task<ApplicationTestResponse> SendGetToApplication(Uri uri);
    }
}