using System;
using System.Linq;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Store.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private IProjectManager _projectManager;
        private readonly IPlatformAdminUserManager _platformAdminUserManager;
        private readonly IDocumentStore _documentStore;

        private readonly IFileManager _fileManager;

        public MediaController(IProjectManager projectManager, IPlatformAdminUserManager platformAdminUserManager, IDocumentStoreHolder documentStoreHolder, IFileManager fileManager)
        {
            _projectManager = projectManager;
            _platformAdminUserManager = platformAdminUserManager;
            _documentStore = documentStoreHolder.Store;
            _fileManager = fileManager;
        }

        [HttpPost]
        [Route("[action]/projects/{projectId}")]
        public async Task<IActionResult> Save([FromForm]IFormFile file, [FromRoute] string projectId)
        {
            // Who's logged in?
            using var session = _documentStore.OpenAsyncSession();
            var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);
            // Which project are we working on?
            var project = await _projectManager.Get((ProjectId)projectId);
            // Does the user have access to the project?
            if (!project.AdminIds.Contains(user.Id) && project.OwnerAdminId != user.Id)
            {
                throw new ApiException("Seems you are not an admin on this project.", (int)System.Net.HttpStatusCode.Unauthorized);
            }
            try
            {

                var files = Request.Form.Files;

                return Ok(await _fileManager.SaveAsync(file));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}