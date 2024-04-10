using ASP.Web.Features.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASP.Web.Features.AnalyticsTrackingPreferences
{
    public class AnalyticsTrackingActionFilter : ActionFilterAttribute
    {
        private readonly ICookieProvider? _cookieProvider;

        public AnalyticsTrackingActionFilter(ICookieProvider cookieProvider)
        {
            _cookieProvider = cookieProvider;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;

            var analyticsTracking = _cookieProvider?.GetCookie(CookieKeys.AnalyticsTracking);
            var analyticsTrackingConfirmation = _cookieProvider?.GetCookie(CookieKeys.AnalyticsTrackingConfirmation);

            TrackingPreferencesModel preferences = new()
            {
                AnalyticsTracking = Enum.TryParse(analyticsTracking,
                             out AnalyticsTracking resultTracking) == true ? resultTracking : AnalyticsTracking.NotSet,
                AnalyticsTrackingConfirmation = Enum.TryParse(analyticsTrackingConfirmation,
                              out AnalyticsTrackingConfirmation resultConfirm) == true ? resultConfirm : AnalyticsTrackingConfirmation.HideBanner
            };

            controller?.ViewData.Add("AnalyticsTrackingPreferences", preferences);
        }
    }
}