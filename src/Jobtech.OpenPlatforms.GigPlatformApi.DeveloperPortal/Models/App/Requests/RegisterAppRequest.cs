using System.ComponentModel.DataAnnotations;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Models.App.Requests
{
    public class RegisterAppRequest
    {
        public string AppName { get; set; }
        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }
        public string Description { get; set; }
    }

    public static partial class AppExtensions
    {
        public static CvApp AsApp(this RegisterAppRequest registerAppRequest) 
            => new CvApp {
                AppName = registerAppRequest.AppName,
                ContactEmail = registerAppRequest.ContactEmail,
                Description = registerAppRequest.Description
            };
    }
}
