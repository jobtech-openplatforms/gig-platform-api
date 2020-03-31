using System.Collections.Generic;
using System.Linq;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    public class CreateProjectRequest
    {
        public string Name { get; set; }
        public bool TestMode { get; set; }

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

        public static Project ToEntity(this CreateProjectModel createProjectModel) 
            => new Project
                {
                    Name = createProjectModel.Name,
                    OwnerAdminId = createProjectModel.OwnerAdminId,
                    AdminIds = createProjectModel.AdminIds,
                    Platforms = new List<Platform> { Platform.Create() },
                    Applications = new List<Application> () // TODO: Create application elsewhere and ensure the entity is updated  
                };

        public static TestProject ToTestEntity(this Project project) 
            => new TestProject {
                LiveProjectId = project.Id,
                Name = project.Name,
                AdminIds = project.AdminIds,
                Applications = project.Applications?
                                        .Select(application => 
                                            new Application { 
                                                    Id = new string(application.Id.ToCharArray().OrderBy(s => ((new System.Random()).Next(2) % 2) == 0).ToArray()), 
                                                    SecretKey = System.Guid.NewGuid().ToString() 
                                            }) 
                                        ??
                                        new List<Application> {
                                            new Application
                                            {
                                                Id = "TEST" + RandomIdString(28),
                                                SecretKey = System.Guid.NewGuid().ToString()
                                            }
                                        },
                Description = project.Description,
                LogoUrl = project.LogoUrl,
                OwnerAdminId = project.OwnerAdminId,
                Platforms = project.Platforms?
                                        .Select(platform =>
                                            new Platform
                                            {
                                                Id = System.Guid.NewGuid(),
                                                PlatformToken = System.Guid.NewGuid().ToString(),
                                                Published = false
                                            })
                                            ??
                                            new List<Platform> {
                                                new Platform
                                                {
                                                    Id = System.Guid.NewGuid(),
                                                    PlatformToken = System.Guid.NewGuid().ToString(),
                                                    Published = false
                                                }
                                            },
                Webpage = project.Webpage
            };

        private static string RandomIdString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new System.Random();

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}