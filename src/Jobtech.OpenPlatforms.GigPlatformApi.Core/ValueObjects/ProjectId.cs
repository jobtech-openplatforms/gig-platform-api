using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects
{
    public class ProjectId : StringIdentity<Project>
    {
        public static implicit operator ProjectId(string value) => new ProjectId { Value= value };
    }
}