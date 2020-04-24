using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    public abstract class ApplicationRequest
    {
        public string AuthCallbackUrl { get; set; }
        public string DataUpdateCallbackUrl { get; set; }
        //public string EmailVerificationUrl { get; set; }
        
        public static implicit operator Application(ApplicationRequest applicationRegistrationRequest) => new Application {
            AuthCallbackUrl = applicationRegistrationRequest.AuthCallbackUrl,
            DataUpdateCallbackUrl = applicationRegistrationRequest.DataUpdateCallbackUrl,
            //EmailVerificationUrl = applicationRegistrationRequest.EmailVerificationUrl
        };

        public Application CreateApplication(CreateApplicationResult createApplicationResult)
        => new Application
        {
            Id = createApplicationResult.ApplicationId,
            AuthCallbackUrl = this.AuthCallbackUrl,
            DataUpdateCallbackUrl = this.DataUpdateCallbackUrl,
            //EmailVerificationUrl = this.EmailVerificationUrl,
            //ApplicationId = createApplicationResult.ApplicationId,
            SecretKey = createApplicationResult.SecretKey
        };
    }
}