using System;
using System.Collections.Generic;

namespace AF.GigPlatform.Core.Entities
{
    public class PlatformAdminUser : PlatformAdmin
    {
        public string UniqueIdentifier { get; set; }

        //TODO: remove password stuff
        //public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        //public ICollection<Platform> Platforms { get; set; }
        public PasswordReset PasswordReset { get; set; }
    }

    public class PlatformAdmin
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

    }

    public class PasswordReset
    {
        public DateTimeOffset? Requested { get; set; }
        public string ResetCode { get; set; }
    }
}