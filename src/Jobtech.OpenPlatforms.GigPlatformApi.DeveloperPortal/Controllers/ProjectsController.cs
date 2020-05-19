using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService;
using Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Extensions;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Helpers;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectManager _projectManager;
        private readonly IProjectUpdateManager _projectUpdateManager;
        private readonly IPlatformAdminHttpClient _platformAdminHttpClient;
        private readonly IApplicationHttpClient _applicationHttpClient;
        private readonly IPlatformAdminUserManager _platformAdminUserManager;
        private readonly IDocumentStore _documentStore;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(IProjectManager projectManager,
            IProjectUpdateManager projectUpdateManager,
            IPlatformAdminHttpClient platformAdminHttpClient,
            IApplicationHttpClient applicationHttpClient,
            IPlatformAdminUserManager platformAdminUserManager,
            IDocumentStore documentStoreHolder,
            ILogger<ProjectsController> logger)
        {
            _projectManager = projectManager;
            _projectUpdateManager = projectUpdateManager;
            _platformAdminHttpClient = platformAdminHttpClient;
            _applicationHttpClient = applicationHttpClient;
            _platformAdminUserManager = platformAdminUserManager;
            _documentStore = documentStoreHolder;
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request)
        {
            var requestName = request.Name.Trim();

            if (string.IsNullOrEmpty(requestName) || string.IsNullOrWhiteSpace(requestName))
            {
                return BadRequest(new { message = "You have to enter a project name." });
            }
            try
            {
                _logger.LogInformation("Creating project");
                using var session = _documentStore.OpenAsyncSession();
                var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);
                // TODO: Do not create platform on LIVE project when creating the entity. Instead do it after.
                var platform = await _platformAdminHttpClient.CreatePlatform(new CreatePlatformModel
                {
                    AuthMechanism = PlatformAuthenticationMechanism.Email,
                    Name = requestName,
                    MaxRating = 5,
                    MinRating = 1,
                    RatingSuccessLimit = 3
                });

                var application = await _applicationHttpClient.CreateApplication(new CreateApplicationModel { Name = requestName });

                var entityToCreate = request.WithOwner(user.Id).ToEntity(
                            Core.Entities.Platform.Create(platform.PlatformId), 
                            new Core.Entities.Application { 
                                Id = application.ApplicationId, 
                                SecretKey = application.SecretKey
                            }
                            );

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
                return Ok(new { projects = projects.OrderBy(p => p.Name).ToList(), testProjects = testProjects.OrderBy(p => p.Name).ToList() });
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Unable to retrieve projects.");
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> PlatformUrl([FromBody] UpdatePlatformUrlRequest request)
        {
            //var errors = Util.UriErrors(new Dictionary<string, string>
            //{ { "platform-url", request.Url },
            //}, _logger);
            //if (errors != null && errors.Any())
            //{
            //    return BadRequest(new { message = "All urls have to be valid.", errors = errors });
            //}
            try
            {
                // Who's logged in?
                using var session = _documentStore.OpenAsyncSession();
                var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);

                var testMode = !ProjectId.IsValidIdentity(request.ProjectId);

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
                    _logger.LogCritical("Platform not found.");
                    throw new ApiException("Platform not found. Please contact your admin or create a new project.", (int)System.Net.HttpStatusCode.NotFound);
                }
                else if (project.Platforms.FirstOrDefault().ExportDataUri == request.Url)
                {
                    // No change. Return.
                    return Ok(project);
                }
                else
                {
                    if (!testMode)
                    {
                        var existingPlatform = await _platformAdminHttpClient.GetPlatform(project.Platforms.FirstOrDefault().Id);
                        platform = new Core.Entities.Platform { Id = existingPlatform.PlatformId, PlatformToken = project.Platforms.FirstOrDefault().PlatformToken, ExportDataUri = request.Url, LastUpdate = DateTime.UtcNow, Published = !existingPlatform.IsInactive };
                    }
                    else
                    {
                        platform = project.Platforms.FirstOrDefault();
                        platform.ExportDataUri = request.Url;
                    }
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
        public async Task<IActionResult> Update([FromBody] UpdateProjectRequest request)
        {
            using var session = _documentStore.OpenAsyncSession();
            var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);

            var project = await _projectUpdateManager.Update(request, session, user.Id);

            return Ok(project);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> PlatformLive([FromBody] UpdatePlatformStatusRequest request)
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

            var errors = this.stringErrors(new Dictionary<string, string>
            { { "project-name", project.Name },
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
                    await _platformAdminHttpClient.GetPlatform(new ProjectModel { PlatformId = platformId });
            }
            catch (ApiException ex)
            {
                _logger.LogInformation("InnerException {exception} status code {statusCode}", ex.InnerException, ex.StatusCode);
                if (ex.InnerException is System.Net.Http.HttpRequestException && ex.StatusCode == 404)
                {
                    // The platform wasn't found - see if we can create it
                    _logger.LogInformation(ex, "Unable to get platform {platformId} - attempting creation", project.Platforms?.FirstOrDefault()?.Id);
                    var apiPlatform = await _platformAdminHttpClient.CreatePlatform(new CreatePlatformModel
                    {
                        AuthMechanism = PlatformAuthenticationMechanism.Email,
                        Name = project.Name,
                        MaxRating = 5,
                        MinRating = 1,
                        RatingSuccessLimit = 3,
                        Description = project.Description,
                        LogoUrl = project.LogoUrl,
                        WebsiteUrl = project.Webpage
                    });
                    if (apiPlatform == null)
                    {
                        _logger.LogCritical(ex, "Unable to create platform for project {projectId}", project.Id);
                        throw;
                    }
                    platformId = apiPlatform.PlatformId.ToString();
                    project.Platforms = new List<Core.Entities.Platform> { Core.Entities.Platform.Create(apiPlatform.PlatformId, project.Platforms.FirstOrDefault().ExportDataUri) };
                    _logger.LogInformation("Created platform {platformId} and added to project {projectId}", apiPlatform.PlatformId, project.Id);
                }
                else
                {
                    _logger.LogCritical(ex, "Unable to get platform status for project {projectId} platform {platformId}", project.Id, project.Platforms?.FirstOrDefault()?.Id);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unable to retrieve platform status for project {projectId} platform {platformId}", project.Id, project.Platforms?.FirstOrDefault()?.Id);
                throw new ApiException(ex, 500);
            }

            if (request.Status == "deactivate")
            {
                await _platformAdminHttpClient.DeactivatePlatform(new ProjectModel { PlatformId = platformId });
                project.DeactivatePlatform();
            }
            else
            {
                await _platformAdminHttpClient.ActivatePlatform(new ProjectModel { PlatformId = platformId });
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