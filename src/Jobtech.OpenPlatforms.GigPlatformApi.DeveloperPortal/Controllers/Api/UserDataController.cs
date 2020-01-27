using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models.PlatformModels;
using Jobtech.OpenPlatforms.GigPlatformApi.EventDispatcher.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        ///
        /// </summary>
        /// <param name="platformDispatchManager"></param>
        /// <param name="platformManager"></param>
        public UserDataController(IPlatformDispatchManager platformDispatchManager, IPlatformManager platformManager)
        {
            _platformDispatchManager = platformDispatchManager;
            _platformManager = platformManager;
        }

        /// <summary>
        /// Endpoint for Platform to send updated data about a user.
        /// Normally used after a request sent to the Platform.
        /// The Platform is expected to return the requestId from the initial request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> Update(PlatformUserDataResult request)
        {
            // Get platform
            var platform = await _platformManager.GetPlatformByTokenAsync(request.PlatformToken);

            // If there is no platform found, maybe this didn't come from a registered platform
            if (platform == null)
            {
                // TODO: Log this

                return BadRequest($"There is no platform registered with the token provided.");
            }

            //var message = new PlatformUserUpdateDataMessage(
            //                    request.RequestId,
            //                    request.Username,
            //                    DateTimeOffset.Now
            //                    );

            ////var message = new PlatformUserDataMessage(
            ////                    request.RequestId,
            ////                    request.Username,
            ////                    DateTimeOffset.Now,
            ////                    new PlatformData(
            ////                        request.Result?.NumberOfGigs ?? 0,
            ////                        request.Result?.PeriodStart,
            ////                        request.Result?.PeriodEnd,
            ////                        request.Result?.Ratings?.Select(s => new PlatformRating(s.Value, s.Created)),
            ////                        request.Result?.Reviews?.Select(s => new PlatformReview(s.ReviewText, s.ReviewerName, s.ReviewHeading, s.ReviewDate)) ?? new List<PlatformReview>()
            ////                        )
            ////                    );
            //await _platformDispatchManager.SendUserDataMessage(message);

            return Ok("Update received.");
        }
    }
}