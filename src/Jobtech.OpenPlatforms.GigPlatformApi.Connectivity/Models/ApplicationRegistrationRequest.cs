using AF.GigPlatform.Core.Entities;
using AF.GigPlatform.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AF.GigPlatform.Connectivity.Models
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
