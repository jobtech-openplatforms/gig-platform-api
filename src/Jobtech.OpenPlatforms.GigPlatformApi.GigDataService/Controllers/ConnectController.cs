using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Microsoft.AspNetCore.Mvc;

namespace Jobtech.OpenPlatforms.GigPlatformApi.GigDataService.Controllers
{
    /// <summary>
    /// API endpoints for connection between platform and user
    /// TODO: Delete this controller 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectController : ControllerBase
    {
        private readonly IPlatformManager _platformManager;
        private readonly IConnectionManager _connectionManager;
        private readonly IConnectionUserManager _connectionUserManager;

        public ConnectController(IPlatformManager platformManager, IConnectionManager connectionManager, IConnectionUserManager connectionUserManager)
        {
            _platformManager = platformManager;
            _connectionManager = connectionManager;
            _connectionUserManager = connectionUserManager;
        }

        //[HttpPost("user-to-platform")]
        //public async Task<IActionResult> ConnectUserToPlatform([FromBody] ConnectUserToPlatformRequest request)
        //{
        //    _connectionUserManager.get
        //}

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpPost("user")]
        //[Obsolete("This version is no longer used since the service won't have its own web interface.")]
        //public async Task<IActionResult> ConnectUser([FromBody] ConnectionRequest request)
        //{
        //    List<string> progress = new List<string>();
        //    // Should this be looking in the session for the user's account?
        //    var model = new ConnectionRequest
        //    {
        //        PlatformId = request.PlatformId.StartsWith("platforms/") ? request.PlatformId : "platforms/" + request.PlatformId,
        //        UserId = request.UserId.StartsWith("users/") ? request.UserId : "users/" + request.UserId,
        //    };

        //    if (!await _connectionManager.ConnectionExistsAsync(new PlatformId { Value = model.PlatformId }, new UserId { Value = model.UserId }))
        //        return BadRequest(new
        //        {
        //            message = "Seems like this connection between you and the platform already exists."
        //        });
        //    progress.Add("Connection validated.");

        //    var platform = await _platformManager.GetPlatformAsync(model.PlatformId);

        //    if (platform == null)
        //        return BadRequest(new
        //        {
        //            message = "Seems like this platform does not exist."
        //        });
        //    progress.Add("Platform valid.");

        //    var user = await _connectionUserManager.GetUserAsync((UserId)model.UserId);

        //    if (user == null)
        //        return BadRequest(new
        //        {
        //            message = "Something went wrong with your connection attempt. I couldn't get the account information."
        //        });
        //    progress.Add("User valid.");

        //    var conn = await _connectionManager.SynchronizeConnectionAsync(platform, user);

        //    return Ok();
        //}
    }
}