using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models.PlatformModels;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Controllers
{
    /// <summary>
    /// API endpoings for Platform WebAdmin
    /// **Mostly obsolete** endpoints, pending refactoring
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformHttpClient _platformHttpClient;
        private readonly IPlatformManager _platformManager;
        private IMapper _mapper;

        public PlatformController(IPlatformHttpClient platformHttpClient, IPlatformManager platformManager, IMapper mapper)
        {
            _platformHttpClient = platformHttpClient;
            _platformManager = platformManager;
            _mapper = mapper;
        }

        [HttpPost("test/{id}")]
        public async Task<IActionResult> TestApi([FromRoute] string id, [FromBody] UserDataTestApiModel request)
        {
            // Get platform
            var platform = await _platformManager.GetPlatformAsync(id);


            // If there is no connection, check that the platform exists
            if (platform == null)
            {
                // TODO: Log this
                return NotFound(PlatformUserDataResponse.Fail("", $"No platform found with ID '{id}'."));
            }

            if (string.IsNullOrEmpty(platform.ExportDataUri))
            {
                throw new ApiException(message: "The platform setup is incomplete.", errors: new List<string> { "Missing ExportDataUri." });
            }

            UserDataRequest userDataRequest = new UserDataRequest(platform.PlatformToken, request.Username, Guid.NewGuid().ToString());

            var response = await _platformHttpClient.TestUserDataFromPlatformAsync(userDataRequest, platform.ExportDataUri);

            return Ok(response);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var platforms = await _platformManager.GetPlatformsAsync();
            return Ok(platforms.AsResponse());
        }

        [HttpGet("")]
        public async Task<Core.Entities.Platform> Get([FromHeader]string platformToken)
            => await _platformManager.GetPlatformByTokenAsync(platformToken);

        [HttpGet("{id}")]
        public async Task<Core.Entities.Platform> Get([FromHeader]string platformToken, [FromRoute] string id)
            => await _platformManager.GetPlatformAsync(id);

        // POST api/platform
        // Registers the push notification URI for the platform
        [HttpPost("{id}")]
        public async Task<Core.Entities.Platform> UpdatePlatform([FromRoute] PlatformId id, [FromBody] PlatformRequest platformRequest)
            => await _platformManager.UpdatePlatformAsync(id, platformRequest);
    }
}