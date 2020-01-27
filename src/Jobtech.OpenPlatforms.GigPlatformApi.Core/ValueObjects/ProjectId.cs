using AF.GigPlatform.Core.Entities;

namespace AF.GigPlatform.Core.ValueObjects
{
    public class ProjectId : StringIdentity<Project>
    {
        public static implicit operator ProjectId(string value) => new ProjectId { Value= value };
    }
}