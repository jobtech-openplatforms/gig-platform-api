using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<MediaController> _logger;

    public MediaController(IProjectManager projectManager,
        IPlatformAdminUserManager platformAdminUserManager,
        IDocumentStore documentStoreHolder,
        IFileManager fileManager,
        ILogger<MediaController> logger)
    {
      _projectManager = projectManager;
      _platformAdminUserManager = platformAdminUserManager;
      _documentStore = documentStoreHolder;
      _fileManager = fileManager;
      _logger = logger;
    }

    [HttpPost]
    [Route("[action]/{projectNamespace:alpha}/{projectId}")]
    public async Task<IActionResult> Save(IFormFile file, [FromRoute] string projectNamespace, [FromRoute] string projectId,
        CancellationToken cancellationToken)
    {
      if (!string.Equals(file.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
          !string.Equals(file.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
          !string.Equals(file.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
          !string.Equals(file.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
          !string.Equals(file.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
          !string.Equals(file.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
      {
        throw new Exception("Incorrect file type");
      }

      // Who's logged in?
      using var session = _documentStore.OpenAsyncSession();
      var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);

      projectId = $"{projectNamespace}/{projectId}";

      var testMode = TestProjectId.IsValidIdentity(projectId) && !ProjectId.IsValidIdentity(projectId);

      // Which project are we working on?
      var project = testMode ?
          await _projectManager.GetTest((TestProjectId)projectId, session) :
          await _projectManager.Get((ProjectId)projectId, session);

      string fileExtension;

      switch (file.ContentType)
      {
        case "image/jpg":
        case "image/jpeg":
        case "image/pjpeg":
          fileExtension = ".jpg";
          break;
        case "image/x-png":
        case "image/png":
          fileExtension = ".png";
          break;
        case "image/gif":
          fileExtension = ".gif";
          break;
        default:
          throw new Exception("Incorrect file type");
      }

      var fileName = $"{Guid.NewGuid()}{fileExtension}";

      // Does the user have access to the project?
      if (!project.AdminIds.Contains(user.Id) && project.OwnerAdminId != user.Id)
      {
        throw new ApiException("Seems you are not an admin on this project.", (int)System.Net.HttpStatusCode.Unauthorized);
      }
      try
      {
        return Ok(await _fileManager.UploadFileAsync(file, fileName, $"devprojects/assets"));
      }
      catch (Exception e)
      {
        _logger.LogError(e, "Unable to upload file.");
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }
  }
}