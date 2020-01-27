using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Store.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ApplicationController : ControllerBase
    {
        private IApplicationManager _applicationManager;
        private readonly IDocumentStore _documentStore;
        private readonly IPlatformAdminUserManager _platformAdminUserManager;

        public ApplicationController(
            IApplicationManager applicationManager, IPlatformAdminUserManager platformAdminUserManager, IDocumentStoreHolder documentStoreHolder)
        {
            _applicationManager = applicationManager;
            _platformAdminUserManager = platformAdminUserManager;
            _documentStore = documentStoreHolder.Store;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]ApplicationRegistrationRequest registrationModel)
        {

            try
            {
                var app = await _applicationManager.CreateApplicationAsync(registrationModel);
                return Ok(app);
            }
            catch (ApiException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get all applications this user has access to
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            using var session = _documentStore.OpenAsyncSession();
            var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);

            var applications = await _applicationManager.GetApplicationsAsync(user.Id);
            return Ok(applications.AsResponse());
        }
    }
}