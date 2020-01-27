using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Messages;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models.PlatformModels;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.EventDispatcher.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Jobtech.OpenPlatforms.GigPlatformApi.GigDataService.Controllers
{
    /// <summary>
    /// API endpoints for Platform
    /// </summary>
    //[AuthorizationFilter("#^VJY0!K._bM9?6'9|<)^No]TipJ.-fuFxG(nY&{p#!#Gs?#QVC4mA07%sZfKG.")]
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformHttpClient _platformHttpClient;
        private readonly IPlatformManager _platformManager;
        private readonly IPlatformDispatchManager _platformDispatchManager;
        private readonly ILogger<PlatformController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="platformManager"></param>
        /// <param name="platformHttpClient"></param>
        public PlatformController(
            IPlatformManager platformManager,
            IPlatformHttpClient platformHttpClient,
            IPlatformDispatchManager platformDispatchManager,
            ILogger<PlatformController> logger)
        {
            _platformHttpClient = platformHttpClient;
            _platformManager = platformManager;
            _platformDispatchManager = platformDispatchManager;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint for requesting the latest data for a user from a specific platform
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        // GET api/platform/latest
        [HttpPost("latest")]
        public async Task<IActionResult> RequestLatest([FromBody] UserUpdateRequest request)
        {
            // Create an identifier this particular request
            var requestId = Guid.NewGuid().ToString();

            // Log the request
            _logger.LogInformation("Request for latest update {request}", request);
            // Get platform
            var platform = await _platformManager.GetPlatformAsync(request.PlatformId);

            // If there is no connection, check that the platform exists
            if (platform == null)
            {
                _logger.LogWarning("Platform not found {id}", request.PlatformId);
                return NotFound(PlatformUserDataResponse.Fail(requestId, $"There is no platform registered with ID '{request.PlatformId}'."));
            }

            if (string.IsNullOrEmpty(platform.ExportDataUri))
            {
                throw new ApiException(message: "The platform setup is incomplete.", errors: new List<string> { "Missing ExportDataUri." });
            }

            UserDataRequest userDataRequest = new UserDataRequest(platform.PlatformToken, request.Username, requestId);
            PlatformUserUpdateDataMessage message = null;
            try
            {
                var response = await _platformHttpClient.GetUserDataFromPlatformAsync(userDataRequest, platform.ExportDataUri);

                try
                {
                    message = new PlatformUserUpdateDataMessage(requestId, request.Username, Guid.Parse(request.PlatformId), response);
                    await _platformDispatchManager.SendUserDataMessage(message);
                    return Ok(PlatformUserDataResponse.Ok(requestId, $"Request for update received and dispatched to service bus."));
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Unhandled exception in messaging");
                    throw new ApiException(message: ex.Message, errors: new List<string> { ex.StackTrace });
                    //return BadRequest(PlatformUserDataResponse.Fail(requestId, ex.Message));
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception in platform retrieval");
                throw new ApiException(message: "Unable to retrieve platform from database", errors: new List<string> { platform.ExportDataUri, ex.Message, ex.StackTrace, ex.InnerException?.Message ?? "", JsonConvert.SerializeObject(userDataRequest), JsonConvert.SerializeObject(message) });
            }
            //var response = await _platformHttpClient.RequestUserDataFromPlatformAsync(userDataRequest, platform.ExportDataUri);
            //if (response.Success == false)
            //{
            //    // TODO: Log this
            //    return BadRequest(PlatformUserDataResponse.Fail(requestId, response));
            //}
        }
    }
}