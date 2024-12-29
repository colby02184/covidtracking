using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;

namespace HealthMonitor.Dashboard.Controllers
{
    public class AuthController : ControllerBase
    {
        [HttpGet("login")]
        public IActionResult Login([FromQuery] string? returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return LocalRedirect(returnUrl ?? Url.Content("~/"));
            }

            return Challenge(OktaDefaults.MvcAuthenticationScheme);
        }

        [HttpGet("logout")]
        public IActionResult Logout([FromQuery] string? returnUrl)
        {
        if (!User.Identity.IsAuthenticated)
        {
            return LocalRedirect(returnUrl ?? Url.Content("~/"));
        }

        return SignOut(
            new AuthenticationProperties { RedirectUri = Url.Content("~/") },
            new[]
            {
                OktaDefaults.MvcAuthenticationScheme,
                CookieAuthenticationDefaults.AuthenticationScheme,
            });
    }
    }
}
