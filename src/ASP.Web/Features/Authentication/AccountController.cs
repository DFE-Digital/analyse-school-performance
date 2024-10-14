using ASP.Web.Areas.Home;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Features.Authentication
{
    [Route("account")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("sign-out")]
        public IActionResult Logout()
        {
            if (!(User?.Identity?.IsAuthenticated ?? false))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home", new { area = "Home" });
            }

            return SignOut(
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme
            );
        }
        
        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, OpenIdConnectDefaults.AuthenticationScheme);
        }
        
        [HttpGet]
        [Route("auth/status")]
        public IActionResult CheckAuthStatus()
        {
            if (User.Identity is { IsAuthenticated: true })
            {
                return Ok();
            }
            return Unauthorized();
        }
    }
}