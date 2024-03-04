using ASP.Core.Enums;
using ASP.Web.Controllers;
using ASP.Web.Models;
using ASP.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASP.Web.Filters
{
    public class CheckCookies : ActionFilterAttribute
    {
        private readonly ICookieProvider? _cookieProvider;

        public CheckCookies(ICookieProvider cookieProvider)
        {
            _cookieProvider = cookieProvider;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;

            var analyticsTracking = _cookieProvider?.GetCookie(CookiesController.AnalyticsTrackingCookie);
            var analyticsTrackingConfirmation = _cookieProvider?.GetCookie(CookiesController.AnalyticsTrackingConfirmationCookie);

            CookiePreferencesModel cookiePreferences = new()
            {
                AnalyticsTracking = Enum.TryParse(analyticsTracking,
                             out AnalyticsTracking resultTracking) == true ? resultTracking : AnalyticsTracking.NotSet,
                AnalyticsTrackingConfirmation = Enum.TryParse(analyticsTrackingConfirmation,
                              out AnalyticsTrackingConfirmation resultConfirm) == true ? resultConfirm : AnalyticsTrackingConfirmation.HideBanner           
             };

            controller?.ViewData.Add("CookiePreferences", cookiePreferences);
        }
    }
}