using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    public class ApplicationRegistrationRequest
    {
        public string AuthCallbackUrl { get; set; }
        public string GigDataNotificationUrl { get; set; }
        public string EmailVerificationUrl { get; set; }

        public static implicit operator Application(ApplicationRegistrationRequest applicationRegistrationRequest) => new Application {
            AuthCallbackUrl = applicationRegistrationRequest.AuthCallbackUrl,
            GigDataNotificationUrl = applicationRegistrationRequest.GigDataNotificationUrl,
            EmailVerificationUrl = applicationRegistrationRequest.EmailVerificationUrl
        };
    }
}
