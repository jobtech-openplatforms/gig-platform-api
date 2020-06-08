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
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;

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
        private readonly IDocumentStore _documentStore;
        private IMapper _mapper;
        private readonly ILogger<PlatformController> _logger;

        public PlatformController(IPlatformHttpClient platformHttpClient, IPlatformManager platformManager, IMapper mapper,
            IDocumentStore documentStoreHolder,
            ILogger<PlatformController> logger)
        {
            _platformHttpClient = platformHttpClient;
            _platformManager = platformManager;
            _mapper = mapper;
            _documentStore = documentStoreHolder;
            _logger = logger;
        }

        [HttpPost("test/{id}")]
        public async Task<IActionResult> TestApi([FromRoute] string id, [FromBody] UserDataTestApiModel request)
        {
            _logger.LogInformation("Testing platform {id}: {@request}", id, request);


            using var session = _documentStore.OpenAsyncSession();
            // Get platform
            var platform = await _platformManager.GetPlatformAsync(id, session);

            // If there is no connection, check that the platform exists
            if (platform == null)
            {
                // TODO: Log this
                return NotFound(PlatformUserDataResponse.Fail("", $"No platform found with ID '{id}'. [-]"));
            }

            if (string.IsNullOrEmpty(platform.ExportDataUri))
            {
                throw new ApiException(message: "The platform setup is incomplete.", errors: new List<string> { "Missing ExportDataUri." });
            }

            UserDataRequest userDataRequest = new UserDataRequest(platform.PlatformToken, request.UserEmail, Guid.NewGuid().ToString());

            var response = await _platformHttpClient.TestUserDataFromPlatformAsync(userDataRequest, platform.ExportDataUri);

            if(response.Response.Status == "NotFound")
            {
                throw new ApiException(message: "The server responded with NotFound.", errors: new List<string> { "Response: " + response.Response.Body });
                
            }

            return Ok(response);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            using var session = _documentStore.OpenAsyncSession();
            var platforms = await _platformManager.GetPlatformsAsync(session);
            return Ok(platforms.AsResponse());
        }

        [HttpGet("")]
        public async Task<Core.Entities.Platform> Get([FromHeader] string platformToken)
        {
            using var session = _documentStore.OpenAsyncSession();
            return await _platformManager.GetPlatformByTokenAsync(platformToken, session);
        }

        [HttpGet("{id}")]
        public async Task<Core.Entities.Platform> Get([FromHeader] string platformToken, [FromRoute] string id)
        {
            using var session = _documentStore.OpenAsyncSession();
            return await _platformManager.GetPlatformAsync(id, session);
        }

        // POST api/platform
        // Registers the push notification URI for the platform
        [HttpPost("{id}")]
        public async Task<Core.Entities.Platform> UpdatePlatform([FromRoute] PlatformId id, [FromBody] PlatformRequest platformRequest)
        {
            using var session = _documentStore.OpenAsyncSession();
            return await _platformManager.UpdatePlatformAsync(id, platformRequest, session);
        }
    }
}