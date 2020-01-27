using AF.GigPlatform.Core.Entities;

namespace AF.GigPlatform.Core.ValueObjects
{
    public class PlatformToken : StringToken<Platform>
    {
        public PlatformToken New()
            => new PlatformToken { Value = System.Guid.NewGuid().ToString() };

        public static implicit operator PlatformToken(string value) => new PlatformToken { Value = value };
    }
}