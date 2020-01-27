using AF.GigPlatform.Core.Entities;
using System.Collections.Generic;

namespace AF.GigPlatform.Connectivity.Models
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