using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects
{
    public class PlatformAdminUserId : StringIdentity<PlatformAdminUser>
    {
        public static implicit operator PlatformAdminUserId(string value) => new PlatformAdminUserId { Value= value };
    }
}