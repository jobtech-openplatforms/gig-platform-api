using AF.GigPlatform.Core.Entities;
using System;

namespace AF.GigPlatform.Core.ValueObjects
{
    public class PlatformId : GuidIdentity<Platform>
    {
        public static implicit operator PlatformId(string value) => new PlatformId { Value= Guid.Parse(value) };
        public static implicit operator PlatformId(Guid value) => new PlatformId { Value= value };
    }
}