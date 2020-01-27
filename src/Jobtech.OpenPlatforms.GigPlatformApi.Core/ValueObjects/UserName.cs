using AF.GigPlatform.Core.Entities;

namespace AF.GigPlatform.Core.ValueObjects
{
    public class UserName : StringIdentity<User>
    {
        public static implicit operator UserName(string value) => new UserName { Value= value };
    }
}