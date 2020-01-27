using Microsoft.AspNetCore.Mvc;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }        
    }
}