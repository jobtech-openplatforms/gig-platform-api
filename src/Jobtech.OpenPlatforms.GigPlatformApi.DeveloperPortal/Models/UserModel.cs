using System.ComponentModel.DataAnnotations;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Models
{
    public class UserModel
    {
        public string Name { get; set; }
        public string DataVersion { get; set; }
        public string Photo { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } // Unique identifier
    }
}
