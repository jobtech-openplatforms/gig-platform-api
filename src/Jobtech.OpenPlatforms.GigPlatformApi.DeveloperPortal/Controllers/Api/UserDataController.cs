using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models.PlatformModels;
using Jobtech.OpenPlatforms.GigPlatformApi.EventDispatcher.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Controllers.Api
{
    /// <summary>
    /// User Controller for Platforms API
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly IPlatformDispatchManager _platformDispatchManager;
        private readonly IPlatformManager _platformManager;
        private readonly IDocumentStore _documentStore;

        /// <summary>
        ///
        /// </summary>
        /// <param name="platformDispatchManager"></param>
        /// <param name="platformManager"></param>
        /// <param name="documentStoreHolder"></param>
        public UserDataController(IPlatformDispatchManager platformDispatchManager, IPlatformManager platformManager,
                IDocumentStore documentStoreHolder
            )
        {
            _platformDispatchManager = platformDispatchManager;
            _platformManager = platformManager;
            _documentStore = documentStoreHolder;
        }

        /// <summary>
        /// Endpoint for Platform to send updated data about a user.
        /// Normally used after a request sent to the Platform.
        /// The Platform is expected to return the requestId from the initial request.
        /// (Currently not used - abandonded functionality but code remains in case of relevance)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> Update(PlatformUserDataResult request)
        {
                using var session = _documentStore.OpenAsyncSession();
            // Get platform
            var platform = await _platformManager.GetPlatformByTokenAsync(request.PlatformToken, session);

            // If there is no platform found, maybe this didn't come from a registered platform
            if (platform == null)
            {
                // TODO: Log this

                return BadRequest($"There is no platform registered with the token provided.");
            }

            return Ok("Update received.");
        }
    }
}