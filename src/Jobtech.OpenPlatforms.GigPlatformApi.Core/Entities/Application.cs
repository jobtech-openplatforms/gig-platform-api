﻿namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities
{
    public class Application
    {
        public string Id { get; set; }
        public string AuthCallbackUrl { get; set; }
        public string DataUpdateCallbackUrl { get; set; }
        //public string EmailVerificationUrl { get; set; }

        public string SecretKey { get; set; }
    }
}
