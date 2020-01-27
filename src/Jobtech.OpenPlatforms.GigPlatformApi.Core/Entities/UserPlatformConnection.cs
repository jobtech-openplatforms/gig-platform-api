using AF.GigPlatform.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AF.GigPlatform.Core.Entities
{
    public class UserPlatformConnection
    {
        public string Id { get; set; }
        public PlatformId PlatformId { get; set; }
        public PlatformToken PlatformToken { get; set; }
        public UserName Username { get; set; }
    }
}
