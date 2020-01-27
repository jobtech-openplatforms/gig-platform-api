using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    public class UpdateApplicationUrlsRequest
    {
        public string ProjectId { get; set; }
        public string AuthCallbackUrl { get; set; }
        public string GigDataNotificationUrl { get; set; }
        public string EmailVerificationUrl { get; set; }


        public Application CreateApplication(CreateApplicationResult createApplicationResult)
        => new Application
        {
            Id = createApplicationResult.ApplicationId,
            AuthCallbackUrl = this.AuthCallbackUrl,
            GigDataNotificationUrl = this.GigDataNotificationUrl,
            EmailVerificationUrl = this.EmailVerificationUrl,
            //ApplicationId = createApplicationResult.ApplicationId,
            SecretKey = createApplicationResult.SecretKey
        };
        public Application CreateApplication(Application oldApplication)
        => new Application
        {
            Id = oldApplication.Id,
            AuthCallbackUrl = this.AuthCallbackUrl,
            GigDataNotificationUrl = this.GigDataNotificationUrl,
            EmailVerificationUrl = this.EmailVerificationUrl,
            //ApplicationId = createApplicationResult.ApplicationId,
            SecretKey = oldApplication.SecretKey
        };
    }

    public static class UpdateApplicationUrlsExtensions
    {

        public static Application UpdateApplication(this Application application, UpdateApplicationUrlsRequest updateApplicationUrlsRequest)
        => new Application
        {
            Id = application.Id,
            AuthCallbackUrl = updateApplicationUrlsRequest.AuthCallbackUrl,
            GigDataNotificationUrl = updateApplicationUrlsRequest.GigDataNotificationUrl,
            EmailVerificationUrl = updateApplicationUrlsRequest.EmailVerificationUrl,
            //ApplicationId = application.ApplicationId,
            SecretKey = application.SecretKey
        };
    }
}