using AF.GigPlatform.Core.Entities;

namespace AF.GigPlatform.Core.ValueObjects
{
    public class PlatformAdminUserId : StringIdentity<PlatformAdminUser>
    {
        public static implicit operator PlatformAdminUserId(string value) => new PlatformAdminUserId { Value= value };
    }
}