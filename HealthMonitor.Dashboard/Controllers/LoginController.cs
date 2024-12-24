using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace HealthMonitor.Dashboard.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet("Login")]
        public IActionResult Login([FromQuery] string returnUrl)
        {
            var redirectUrl = returnUrl is null ? Url.Content("~/") : "/" + returnUrl;
            if (User.Identity.IsAuthenticated)
            {
                return LocalRedirect(redirectUrl);
            }

            return Challenge();
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout([FromQuery] string returnUrl)
        {
            var redirectUrl = returnUrl is null ? Url.Content("~/") : "/" + returnUrl;
            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect(redirectUrl);
            }
            await HttpContext.SignOutAsync();
            return LocalRedirect(redirectUrl);
        }
    }
}
