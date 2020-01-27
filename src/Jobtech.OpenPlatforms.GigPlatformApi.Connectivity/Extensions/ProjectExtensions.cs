using AF.GigPlatform.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AF.GigPlatform.Connectivity.Extensions
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
