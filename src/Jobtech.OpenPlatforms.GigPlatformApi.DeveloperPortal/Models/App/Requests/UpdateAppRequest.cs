using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Models.App.Requests
{
    public class UpdateAppRequest : RegisterAppRequest
    {
        public string Id { get; set; }
    }

    public static partial class AppExtensions
    {
        public static CvApp AsApp(this UpdateAppRequest updateAppRequest)
            => new CvApp
            {
                Id = updateAppRequest.Id,
                AppName = updateAppRequest.AppName,
                ContactEmail = updateAppRequest.ContactEmail,
                Description = updateAppRequest.Description
            };
    }
}
