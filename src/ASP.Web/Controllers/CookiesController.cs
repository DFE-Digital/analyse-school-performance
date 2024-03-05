using ASP.Core.Enums;
using ASP.Web.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Controllers
{
    [Route("cookies")]
    public class CookiesController(ICookieProvider cookieProvider) : Controller
    {
        private readonly ICookieProvider _cookieProvider = cookieProvider;

        public const string AnalyticsTrackingCookie = "AnalyticsTracking";
        public const string AnalyticsTrackingConfirmationCookie = "AnalyticsTrackingConfirmation";

        [HttpPost("preferences")]
        public IActionResult CookiesPreferences(string analyticsTracking)
        {
            _cookieProvider.SetCookie(AnalyticsTrackingCookie, analyticsTracking);
            _cookieProvider.SetCookie(AnalyticsTrackingConfirmationCookie, AnalyticsTrackingConfirmation.ShowBanner.ToString());

            return Redirect(Request.Headers.Referer.ToString());
        }

        [HttpGet("confirmation")]
        public IActionResult CookiesConfirmation()
        {
            _cookieProvider.SetCookie(AnalyticsTrackingConfirmationCookie, AnalyticsTrackingConfirmation.HideBanner.ToString());

            return Redirect(Request.Headers.Referer.ToString());
        }
    }
}
