using ASP.Web.Features.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Features.AnalyticsTrackingPreferences
{
    [Route("analytics-tracking")]
    public class AnalyticsTrackingPreferencesController(ICookieProvider cookieProvider) : Controller
    {
        private readonly ICookieProvider _cookieProvider = cookieProvider;

        [HttpPost("save-preferences")]
        public IActionResult SavePreferences(string analyticsTracking)
        {
            _cookieProvider.SetCookie(CookieKeys.AnalyticsTracking, analyticsTracking);
            _cookieProvider.SetCookie(CookieKeys.AnalyticsTrackingConfirmation, AnalyticsTrackingConfirmation.ShowBanner.ToString());

            return Redirect(Request.Headers.Referer.ToString());
        }

        [HttpPost("hide-confirmation-banner")]
        public IActionResult HideConfirmationBanner()
        {
            _cookieProvider.SetCookie(CookieKeys.AnalyticsTrackingConfirmation, AnalyticsTrackingConfirmation.HideBanner.ToString());

            return Redirect(Request.Headers.Referer.ToString());
        }
    }
}
