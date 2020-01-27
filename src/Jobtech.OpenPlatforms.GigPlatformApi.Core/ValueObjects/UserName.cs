using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects
{
    public class UserName : StringIdentity<User>
    {
        public static implicit operator UserName(string value) => new UserName { Value= value };
    }
}