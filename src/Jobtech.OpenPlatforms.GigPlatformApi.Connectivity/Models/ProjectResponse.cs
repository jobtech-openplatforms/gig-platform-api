using System.Collections.Generic;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    public class ProjectResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public static implicit operator ProjectResponse(Project project) => new ProjectResponse { Id = project.Id, Name = project.Name };
    }

    public static class ProjectResponseExtensions
    {
        public static ProjectResponse AsResponse(this Project project) => project;

        public static IEnumerable<ProjectResponse> AsResponse(this IEnumerable<Project> projects)
        {
            foreach (var item in projects)
            {
                yield return item.AsResponse();
            }
        }
    }
}