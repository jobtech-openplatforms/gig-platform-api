using System.Collections.Generic;
using System.Linq;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Extensions
{
    public static class ProjectExtensions
    {
        public static Project ActivatePlatform(this Project project)
        {
            var platform = project.Platforms.FirstOrDefault();
            project.Platforms = new List<Platform> { platform.Activate() };
            return project;
        }
        public static Project DeactivatePlatform(this Project project)
        {
            var platform = project.Platforms.FirstOrDefault();
            project.Platforms = new List<Platform> { platform.Deactivate() };
            return project;
        }
    }
}
