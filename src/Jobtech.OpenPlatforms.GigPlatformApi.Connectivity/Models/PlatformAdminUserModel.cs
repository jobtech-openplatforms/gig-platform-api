using AF.GigPlatform.Core.Entities;
using System.Collections.Generic;

namespace AF.GigPlatform.Connectivity.Models
{
    public class PlatformAdminUserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}