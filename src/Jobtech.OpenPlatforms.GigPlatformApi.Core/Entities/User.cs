using System.Collections.Generic;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string DataVersion { get; set; } = "1.0";
        public string Photo { get; set; } = "";
        public string Email { get; set; }
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
    
}