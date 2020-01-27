using AF.GigPlatform.Core.Entities;

namespace AF.GigPlatform.Core.ValueObjects
{
    public class UserId : StringIdentity<User>
    {
        public static implicit operator UserId(string value) => new UserId { Value= value };
    }
}