using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects
{
    public class UserId : StringIdentity<User>
    {
        public static implicit operator UserId(string value) => new UserId { Value= value };
    }
}