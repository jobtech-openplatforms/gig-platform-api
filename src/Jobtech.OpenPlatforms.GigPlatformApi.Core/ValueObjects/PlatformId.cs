using System;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects
{
    public class PlatformId : GuidIdentity<Platform>
    {
        public static implicit operator PlatformId(string value) => new PlatformId { Value= Guid.Parse(value) };
        public static implicit operator PlatformId(Guid value) => new PlatformId { Value= value };
    }
}