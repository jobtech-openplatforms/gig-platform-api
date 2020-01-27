using AF.GigPlatform.Core.Entities;

namespace AF.GigPlatform.Core.ValueObjects
{
    public class ApplicationId : StringIdentity<Application>
    {
        public static implicit operator ApplicationId(string value) => new ApplicationId { Value= value };
    }
}