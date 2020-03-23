using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects
{
    public class TestProjectId : StringIdentity<TestProject>
    {
        public static implicit operator TestProjectId(string value) => new TestProjectId { Value= value };
    }
}