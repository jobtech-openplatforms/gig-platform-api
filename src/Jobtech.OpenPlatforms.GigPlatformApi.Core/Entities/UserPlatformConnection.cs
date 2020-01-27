using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities
{
    public class UserPlatformConnection
    {
        public string Id { get; set; }
        public PlatformId PlatformId { get; set; }
        public PlatformToken PlatformToken { get; set; }
        public UserName Username { get; set; }
    }
}
