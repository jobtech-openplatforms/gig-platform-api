using Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/test/app/")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ApplicationTestController : ControllerBase
    {
        private IProjectManager _projectManager;
        private readonly IApplicationTestHttpClient _applicationTestHttpClient;
        private readonly IDocumentStore _documentStore;
        private readonly IPlatformAdminUserManager _platformAdminUserManager;
        private readonly ILogger<ApplicationTestController> _logger;

        public ApplicationTestController(
            IProjectManager projectManager,
            IApplicationTestHttpClient applicationTestHttpClient,
            IPlatformAdminUserManager platformAdminUserManager,
            IDocumentStore documentStoreHolder,
            ILogger<ApplicationTestController> logger)
        {
            _projectManager = projectManager;
            _applicationTestHttpClient = applicationTestHttpClient;
            _platformAdminUserManager = platformAdminUserManager;
            _documentStore = documentStoreHolder;
            _logger = logger;
        }

        [HttpGet("{projectType}/{id}/auth/")]
        public async Task<IActionResult> Auth([FromRoute]string projectType, [FromRoute]string id)
                => await AuthTest($"{projectType}/{id}", "completed");

        [HttpGet("{projectType}/{id}/auth/cancel")]
        public async Task<IActionResult> AuthCancel([FromRoute]string projectType, [FromRoute]string id)
                => await AuthTest($"{projectType}/{id}", "cancelled");

        private async Task<IActionResult> AuthTest(string projectId, string resultOutcome)
        {
            try
            {
                using var session = _documentStore.OpenAsyncSession();
                var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);
                var testMode = TestProjectId.IsValidIdentity(projectId) && !ProjectId.IsValidIdentity(projectId);
                var project = testMode ? await _projectManager.GetTest((TestProjectId)projectId, session) : await _projectManager.Get((ProjectId)projectId, session);

                var result = await _applicationTestHttpClient.SendAuthCallback(project.Applications.First(), Guid.NewGuid().ToString(), resultOutcome, Guid.NewGuid().ToString());
                return Ok(result);
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Unable to test application. {@request}", projectId);
                // return error message if there was an exception

                return Ok(GenericResponse.Failed(ex.Message, (System.Net.HttpStatusCode)ex.StatusCode));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to test application. {@request}", projectId);
                // return error message if there was an exception

                return Ok(GenericResponse.Failed(ex.Message, System.Net.HttpStatusCode.BadRequest));
            }

        }
    }
}