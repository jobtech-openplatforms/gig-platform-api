using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService;
using Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Extensions;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Store.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private IProjectManager _projectManager;
        private readonly IGigDataHttpClient _gigDataHttpClient;
        private readonly IPlatformAdminUserManager _platformAdminUserManager;
        private readonly IDocumentStore _documentStore;

        public ProjectsController(IProjectManager projectManager,
            IGigDataHttpClient gigDataHttpClient,
            IPlatformAdminUserManager platformAdminUserManager,
            IDocumentStoreHolder documentStoreHolder)
        {
            _projectManager = projectManager;
            _gigDataHttpClient = gigDataHttpClient;
            _platformAdminUserManager = platformAdminUserManager;
            _documentStore = documentStoreHolder.Store;
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
                using var session = _documentStore.OpenAsyncSession();
                var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);
                var entityToCreate = request.WithOwner(user.Id).ToEntity();
                var project = await _projectManager.Create(entityToCreate);
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
            try
            {
                using var session = _documentStore.OpenAsyncSession();
                var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);
                var projects = await _projectManager.GetAll(user.Id, session);
                var testProjects = await _projectManager.GetAllTest(user.Id, session);
                if (projects.Any(p => !testProjects.Any(tp => tp.LiveProjectId == p.Id)))
                {
                    foreach (var project in projects.Where(p => !testProjects.Any(tp => tp.LiveProjectId== p.Id )))
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
            var errors = this.uriErrors(new Dictionary<string, string> {
                { "platform-url", request.Url },
            });
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
                var project = await _projectManager.Get((ProjectId)request.ProjectId);
                // Does the user have access to the project?
                if (!project.AdminIds.Contains(user.Id) && project.OwnerAdminId != user.Id)
                {
                    throw new ApiException("Seems you are not an admin on this project.", (int)System.Net.HttpStatusCode.Unauthorized);
                }

                Core.Entities.Platform platform;

                if (project.Platforms == null || !project.Platforms.Any())
                {
                    var registeredPlatform = await _gigDataHttpClient.CreatePlatform(
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
                project = await _projectManager.Update(project);

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
            using var session = _documentStore.OpenAsyncSession();
            var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);
            var project = await _projectManager.Get((ProjectId)request.Id);
            if (!project.AdminIds.Contains(user.Id) && project.OwnerAdminId != user.Id)
            {
                throw new ApiException("Seems you are not an admin on this project.", (int)System.Net.HttpStatusCode.Unauthorized);
            }

            project.Name = string.IsNullOrEmpty(request.Name) ? project.Name : request.Name;
            project.LogoUrl = request.LogoUrl;
            project.Description = request.Description;
            project.Webpage = request.Webpage;

            project = await _projectManager.Update(project);

            return Ok(project);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> PlatformLive([FromBody]UpdatePlatformStatusRequest request)
        {
            using var session = _documentStore.OpenAsyncSession();
            var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);
            var userId = (PlatformAdminUserId)user.Id;
            var project = await _projectManager.Get((ProjectId)request.ProjectId);
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

            try
            {
                var status =
                    await _gigDataHttpClient.PlatformStatus(new ProjectModel { PlatformId = project.Platforms.FirstOrDefault().Id.ToString() });
            }
            catch (Exception ex)
            {
                // Platform with this ID probably doesn't exist
                // TODO: Add check for actual error
                try
                {
                    var registeredPlatform = await _gigDataHttpClient.CreatePlatform(
                            new CreatePlatformModel
                            {
                                AuthMechanism = PlatformAuthenticationMechanism.Email,
                                Name = project.Name,
                                MaxRating = 5,
                                MinRating = 1,
                                RatingSuccessLimit = 3
                            }
                            );
                    var platform = project.Platforms.FirstOrDefault().RegisteredWithId(registeredPlatform.PlatformId);

                    // One platform per project, so just replace. Create() above also sets the LastUpdated date.
                    project.Platforms = new List<Core.Entities.Platform> { platform };
                    // Save
                    project = await _projectManager.Update(project);
                }
                catch (Exception ex2)
                {
                    throw;
                }
            }

            if (request.Status == "deactivate")
            {
                await _gigDataHttpClient.DeactivatePlatform(new ProjectModel { PlatformId = project.Platforms.FirstOrDefault().Id.ToString() });
                project.DeactivatePlatform();
            }
            else
            {
                await _gigDataHttpClient.ActivatePlatform(new ProjectModel { PlatformId = project.Platforms.FirstOrDefault().Id.ToString() });
                project.ActivatePlatform();
            }

            project = await _projectManager.Update(project);

            return Ok(project);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ApplicationUrls([FromBody]UpdateApplicationUrlsRequest request)
        {
            var errors = this.uriErrors(new Dictionary<string, string> {
                { "auth-callback-url", request.AuthCallbackUrl },
                { "gig-data-notification-url", request.GigDataNotificationUrl },
                { "email-verification-url", request.EmailVerificationUrl },
            });
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
                var project = await _projectManager.Get((ProjectId)request.ProjectId);
                // Does the user have access to the project?
                if (!project.AdminIds.Contains(user.Id) && project.OwnerAdminId != user.Id)
                {
                    throw new ApiException("Seems you are not an admin on this project.", (int)System.Net.HttpStatusCode.Unauthorized);
                }
                var application = project.Applications?.FirstOrDefault();

                if (application == null)
                {
                    var registeredApplication = await _gigDataHttpClient.CreateApplication(new CreateApplicationModel
                    {
                        Name = project.Name,
                        AuthCallbackUrl = request.AuthCallbackUrl,
                        EmailVerificationNotificationEndpointUrl = request.EmailVerificationUrl,
                        NotificationEndpointUrl = request.GigDataNotificationUrl
                    });
                    application = request.CreateApplication(registeredApplication);
                }
                else if (
                        application.AuthCallbackUrl == request.AuthCallbackUrl &&
                        application.EmailVerificationUrl == request.EmailVerificationUrl &&
                        application.GigDataNotificationUrl == request.GigDataNotificationUrl
                    )
                {
                    return Ok(project);
                }
                else
                {
                    application = request.CreateApplication(application);
                }

                // One application per project, so just replace
                project.Applications = new List<Core.Entities.Application> { application };
                // Save
                project = await _projectManager.Update(project);

                return Ok(project);
            }
            catch (ApiException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        private IEnumerable<string> uriErrors(Dictionary<string, string> uriStrings)
        {
            foreach (var uri in uriStrings)
            {
                var valid = false;
                try
                {
                    new Uri(uri.Value);
                    valid = true;
                }
                catch (Exception)
                {
                }
                if (!valid)
                {
                    yield return uri.Key; // Just return the ID of the error field
                }
            }
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