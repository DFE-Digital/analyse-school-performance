using ASP.Core.Enums;

namespace ASP.Web.Models
{
    public class CookiePreferencesModel
    {
        public AnalyticsTracking AnalyticsTracking { get; set; } = AnalyticsTracking.NotSet;
        public AnalyticsTrackingConfirmation AnalyticsTrackingConfirmation { get; set; } = AnalyticsTrackingConfirmation.ShowBanner;
    }
}
