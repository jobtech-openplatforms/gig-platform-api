using System;
using System.Collections.Generic;
using System.Text;

namespace AF.GigPlatform.Core.Entities
{
    public class Project
    {
        public IEnumerable<Application> Applications { get; set; }
        public IEnumerable<Platform> Platforms { get; set; }
        public IEnumerable<string> AdminIds { get; set; }
        public string OwnerAdminId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Webpage { get; set; }
        public string LogoUrl { get; set; }
        public string Id { get; set; }
    }
}
