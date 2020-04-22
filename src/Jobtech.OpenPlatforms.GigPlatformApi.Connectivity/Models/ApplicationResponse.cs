using System.Collections.Generic;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    public class ApplicationResponse
    {
        public string Id { get; set; }
        public string AuthCallbackUrl { get; set; }
        public string DataUpdateCallbackUrl { get; set; }
        //public string EmailVerificationUrl { get; set; }

        public string SecretKey { get; set; }
        //public string ApplicationId { get; set; }
    }
    public static class ApplicationResponseExtensions
    {
        public static ApplicationResponse AsResponse(this Application application)
            => new ApplicationResponse { Id = application.Id,
                AuthCallbackUrl = application.AuthCallbackUrl,
                DataUpdateCallbackUrl = application.DataUpdateCallbackUrl,
                //EmailVerificationUrl = application.EmailVerificationUrl,
                //ApplicationId = application.ApplicationId,
                SecretKey = application.SecretKey
            };

        public static IEnumerable<ApplicationResponse> AsResponse(this IEnumerable<Application> applications)
        {
            foreach (var p in applications)
            {
                yield return p.AsResponse();
            }
        }
    }
}
