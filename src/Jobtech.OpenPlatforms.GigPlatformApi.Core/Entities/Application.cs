using System;
using System.Collections.Generic;
using System.Text;

namespace AF.GigPlatform.Core.Entities
{
    public class Application
    {
        public string Id { get; set; }
        public string AuthCallbackUrl { get; set; }
        public string GigDataNotificationUrl { get; set; }
        public string EmailVerificationUrl { get; set; }

        public string SecretKey { get; set; }
        //public string ApplicationId { get; set; }
    }
}
