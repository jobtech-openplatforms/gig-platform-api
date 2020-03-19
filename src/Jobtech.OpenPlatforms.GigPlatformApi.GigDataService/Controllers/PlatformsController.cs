using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jobtech.OpenPlatforms.GigPlatformApi.GigDataService.Controllers
{
    /// <summary>
    /// API endpoints for platforms data
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformManager _platformManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="platformManager"></param>
        public PlatformsController(IPlatformManager platformManager)
        {
            _platformManager = platformManager;
        }

        /// <summary>
        /// Get all platforms
        /// </summary>
        /// <returns></returns>
        // GET api/platforms
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var platforms = await _platformManager.GetPlatformsAsync();
            return Ok(platforms.AsResponse());
        }

        /// <summary>
        /// Get a specific platform
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlatformResponse>> Get([FromRoute]string id)
        {
            var platform = await _platformManager.GetPlatformAsync((PlatformId)id);
            return Ok(platform.AsResponse());
        }
    }
}
