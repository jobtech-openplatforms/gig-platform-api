using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService;
using Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Extensions;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Helpers;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectManager _projectManager;
        private readonly IGigDataHttpClient _gigDataHttpClient;
        private readonly IPlatformAdminUserManager _platformAdminUserManager;
        private readonly IDocumentStore _documentStore;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(IProjectManager projectManager,
            IGigDataHttpClient gigDataHttpClient,
            IPlatformAdminUserManager platformAdminUserManager,
            IDocumentStore documentStoreHolder,
            ILogger<ProjectsController> logger)
        {
            _projectManager = projectManager;
            _gigDataHttpClient = gigDataHttpClient;
            _platformAdminUserManager = platformAdminUserManager;
            _documentStore = documentStoreHolder;
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody]CreateProjectRequest request)
        {

            if (string.IsNullOrEmpty(request.Name))
            {
                return BadRequest(new { message = "You have to enter a project name." });
            }
            try
            {
                _logger.LogInformation("Creating project");
                using var session = _documentStore.OpenAsyncSession();
                var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);
                var entityToCreate = request.WithOwner(user.Id).ToEntity();
                var project = await _projectManager.Create(entityToCreate);
                try
                {
                    var testEntityToCreate = project.ToTestEntity();
                    var testProject = await _projectManager.Create(testEntityToCreate);
                    if (request.TestMode)
                    {
                        return Ok(testProject);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ye testproject creation failed. Project {@project} Request {@request}", project, request);
                }

                // TODO: Get project and testproject to verify creation

                return Ok(project);
            }
            catch (ApiException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateTestProjectFromLive([FromBody]string projectId)
        {
            try
            {
                using var session = _documentStore.OpenAsyncSession();
                var project = await _projectManager.Get((ProjectId)projectId, session);
                var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);
                var entityToCreate = project.ToTestEntity();
                var testproject = await _projectManager.Create(entityToCreate);
                return Ok(testproject);
            }
            catch (ApiException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GenerateTestProjects()
        {
            try
            {
                using var session = _documentStore.OpenAsyncSession();
                var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);
                var projects = await _projectManager.GetAll(user.Id, session);
                foreach (var item in projects)
                {
                    var project = await _projectManager.Get((ProjectId)item.Id, session);
                    var entityToCreate = project.ToTestEntity();
                    var testProject = await _projectManager.Create(entityToCreate);
                }
                var testProjects = await _projectManager.GetAllTest(user.Id, session);
                return Ok(testProjects);
            }
            catch (ApiException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting projects");
            try
            {
                using var session = _documentStore.OpenAsyncSession();
                var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);
                var projects = await _projectManager.GetAll(user.Id, session);
                var testProjects = await _projectManager.GetAllTest(user.Id, session);
                if (projects.Any(p => !testProjects.Any(tp => tp.LiveProjectId == p.Id)))
                {
                    foreach (var project in projects.Where(p => !testProjects.Any(tp => tp.LiveProjectId == p.Id)))
                    {
                        await _projectManager.Create(project.ToTestEntity());
                    }

                    testProjects = await _projectManager.GetAllTest(user.Id, session);
                }
                return Ok(new { projects, testProjects });
            }
            catch (ApiException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> PlatformUrl([FromBody]UpdatePlatformUrlRequest request)
        {
            var errors = Util.UriErrors(new Dictionary<string, string> {
                { "platform-url", request.Url },
            }, _logger);
            if (errors != null && errors.Any())
            {
                return BadRequest(new { message = "All urls have to be valid.", errors = errors });
            }
            try
            {
                // Who's logged in?
                using var session = _documentStore.OpenAsyncSession();
                var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);

                // Which project are we working on?
                Core.Entities.Project project = (!request.TestMode && ProjectId.IsValidIdentity(request.ProjectId)) ?
                    await _projectManager.Get((ProjectId)request.ProjectId) :

                // if (request.TestMode && TestProjectId.IsValidIdentity(request.ProjectId))
                    await _projectManager.GetTest((TestProjectId)request.ProjectId, session);

                if (project == null)
                {
                    throw new ApiException("Project not found.", (int)System.Net.HttpStatusCode.NotFound);
                }
                // Does the user have access to the project?
                if ((!project.AdminIds.Contains(user.Id)) && project.OwnerAdminId != user.Id)
                {
                    throw new ApiException("Seems you are not an admin on this project.", (int)System.Net.HttpStatusCode.Unauthorized);
                }

                Core.Entities.Platform platform;

                if (project.Platforms == null || !project.Platforms.Any())
                {
                    var registeredPlatform = request.TestMode ?
                        new PlatformViewModel(System.Guid.NewGuid(), project.Name, Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService.PlatformAuthenticationMechanism.Email)
                        : await _gigDataHttpClient.CreatePlatform(
                        new CreatePlatformModel
                        {
                            AuthMechanism = PlatformAuthenticationMechanism.Email,
                            Name = project.Name,
                            MaxRating = 5,
                            MinRating = 1,
                            RatingSuccessLimit = 3
                        }
                        );
                    platform = Core.Entities.Platform.Create(registeredPlatform.PlatformId, request.Url);
                }
                else if (project.Platforms.FirstOrDefault().ExportDataUri == request.Url)
                {
                    // No change. Return.
                    return Ok(project);
                }
                else
                {
                    platform = Core.Entities.Platform.Create(project.Platforms.FirstOrDefault().Id, request.Url);
                }
                // One platform per project, so just replace. Create() above also sets the LastUpdated date.
                project.Platforms = new List<Core.Entities.Platform> { platform };
                // Save
                project = await _projectManager.Update(project, session);

                // TODO: If saving fails, revert back by deleting the platform that was registered with the GigDataService

                return Ok(project);
            }
            catch (ApiException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update([FromBody]UpdateProjectRequest request)
        {
            var requestName = request.Name.Trim();

            if (string.IsNullOrEmpty(requestName) || string.IsNullOrWhiteSpace(requestName))
            {
                throw new ApiException("Seems you are not an admin on this project.", (int)System.Net.HttpStatusCode.BadRequest);
            }

            using var session = _documentStore.OpenAsyncSession();
            var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);

            var testMode = TestProjectId.IsValidIdentity(request.Id) && !ProjectId.IsValidIdentity(request.Id);

            var project = testMode ? await _projectManager.GetTest((TestProjectId)request.Id, session) : await _projectManager.Get((ProjectId)request.Id, session);

            if (!project.AdminIds.Contains(user.Id) && project.OwnerAdminId != user.Id)
            {
                _logger.LogError("User {userId} is not part of {ownerId} or {@adminIds}", user.Id, project.OwnerAdminId, project.AdminIds);
                throw new ApiException("Seems you are not an admin on this project.", (int)System.Net.HttpStatusCode.Unauthorized);
            }

            project.Name = requestName;
            project.LogoUrl = request.LogoUrl;
            project.Description = request.Description;
            project.Webpage = request.Webpage;

            project = await _projectManager.Update(project, session);

            return Ok(project);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> PlatformLive([FromBody]UpdatePlatformStatusRequest request)
        {
            if (!ProjectId.IsValidIdentity(request.ProjectId))
            {
                // Test projects cannot be published
                throw new ApiException("Not a valid project id. Test projects cannot be published.");
            }

            using var session = _documentStore.OpenAsyncSession();
            var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);
            var userId = (PlatformAdminUserId)user.Id;
            var project = await _projectManager.Get((ProjectId)request.ProjectId, session);
            if (!project.AdminIds.Contains(userId.Value) && project.OwnerAdminId != userId.Value)
            {
                throw new ApiException("Seems you are not an admin on this project.", (int)System.Net.HttpStatusCode.Unauthorized);
            }

            var errors = this.stringErrors(new Dictionary<string, string> {
                { "project-name", project.Name },
                { "project-webpage", project.Webpage },
                { "project-description", project.Description },
                { "project-logo", project.LogoUrl },
            });
            if (errors != null && errors.Any())
            {
                throw new ApiException("The project information is incomplete. Please fill out a name, webpage and description, and add a logo to the project, to publish it.", (int)System.Net.HttpStatusCode.ExpectationFailed, errors);
            }

            var platformId = project.Platforms.FirstOrDefault().Id.ToString();

            try
            {
                var apiPlatform =
                    await _gigDataHttpClient.GetPlatform(new ProjectModel { PlatformId = platformId });
            }
            catch (Exception ex)
            {

                _logger.LogCritical(ex, "Unable to retrieve platform status for project {projectId} platform {platformId}", project.Id, project.Platforms?.FirstOrDefault()?.Id);
                throw new ApiException(ex, 500);
            }

            if (request.Status == "deactivate")
            {
                await _gigDataHttpClient.DeactivatePlatform(new ProjectModel { PlatformId = platformId });
                project.DeactivatePlatform();
            }
            else
            {
                await _gigDataHttpClient.ActivatePlatform(new ProjectModel { PlatformId = platformId });
                project.ActivatePlatform();
            }

            project = await _projectManager.Update(project, session);

            return Ok(project);
        }



        private IEnumerable<string> stringErrors(Dictionary<string, string> strings)
        {
            foreach (var str in strings)
            {
                if (string.IsNullOrEmpty(str.Value))
                {
                    yield return str.Key; // Just return the ID of the error field
                }
            }
        }
    }
}