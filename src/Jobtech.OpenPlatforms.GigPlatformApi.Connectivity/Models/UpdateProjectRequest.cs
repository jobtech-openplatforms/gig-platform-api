using AF.GigPlatform.Core.Entities;
using AF.GigPlatform.Core.ValueObjects;

namespace AF.GigPlatform.Connectivity.Models
{
    public class UpdateProjectRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public string Webpage { get; set; }

    }
}