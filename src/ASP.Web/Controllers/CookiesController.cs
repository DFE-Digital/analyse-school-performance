using ASP.Web.Enums;
using ASP.Web.Constants;
using ASP.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Controllers
{
    [Route("cookies")]
    public class CookiesController(ICookieProvider cookieProvider) : Controller
    {
        private readonly ICookieProvider _cookieProvider = cookieProvider;

        [HttpPost("preferences")]
        public IActionResult CookiesPreferences(string analyticsTracking)
        {
            _cookieProvider.SetCookie(CookieKeys.AnalyticsTrackingCookie, analyticsTracking);
            _cookieProvider.SetCookie(CookieKeys.AnalyticsTrackingConfirmationCookie, AnalyticsTrackingConfirmation.ShowBanner.ToString());

            return Redirect(Request.Headers.Referer.ToString());
        }

        [HttpGet("confirmation")]
        public IActionResult CookiesConfirmation()
        {
            _cookieProvider.SetCookie(CookieKeys.AnalyticsTrackingConfirmationCookie, AnalyticsTrackingConfirmation.HideBanner.ToString());

            return Redirect(Request.Headers.Referer.ToString());
        }
    }
}
