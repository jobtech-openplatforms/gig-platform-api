using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects
{
    public class ApplicationId : StringIdentity<Application>
    {
        public static implicit operator ApplicationId(string value) => new ApplicationId { Value= value };
    }
}