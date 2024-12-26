using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace HealthMonitor.Dashboard.Controllers
{
    public class AuthController : ControllerBase
    {
        [HttpGet("login")]
        public IActionResult Login([FromQuery] string returnUrl)
        {
            var redirectUrl = returnUrl is null ? Url.Content("~/") : "/" + returnUrl;
            if (User.Identity.IsAuthenticated)
            {
                return LocalRedirect(redirectUrl);
            }

            return Challenge();
        }

        [HttpGet("logout")]
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
