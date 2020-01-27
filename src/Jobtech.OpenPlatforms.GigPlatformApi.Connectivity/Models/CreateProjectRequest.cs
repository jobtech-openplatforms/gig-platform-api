using AF.GigPlatform.Core.Entities;
using AF.GigPlatform.Core.ValueObjects;
using System.Collections.Generic;

namespace AF.GigPlatform.Connectivity.Models
{
    public class CreateProjectRequest
    {
        public string Name { get; set; }

        public static implicit operator Project(CreateProjectRequest createProjectRequest) => new Project { Name = createProjectRequest.Name };
    }
    public class CreateProjectModel : CreateProjectRequest
    {
        public string CreatedBy { get; set; }
        public string OwnerAdminId { get; set; }
        public string[] AdminIds { get; set; }
    }
    public static class CreateProjectModelExtensions
    {
        public static CreateProjectModel WithOwner(this CreateProjectRequest createProjectRequest, PlatformAdminUserId createdBy)
            => new CreateProjectModel
                {
                    CreatedBy = createdBy.Value,
                    OwnerAdminId = createdBy.Value,
                    AdminIds = new string[] { createdBy.Value },
                    Name = createProjectRequest.Name
                };

        public static Project ToEntity(this CreateProjectModel createProjectModel) => new Project
        {
            Name = createProjectModel.Name,
            OwnerAdminId = createProjectModel.OwnerAdminId,
            AdminIds = createProjectModel.AdminIds,
            Platforms = new List<Platform> { Platform.Create() }
        };
    }
}