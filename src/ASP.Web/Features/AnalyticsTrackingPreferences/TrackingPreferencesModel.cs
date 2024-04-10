namespace ASP.Web.Features.AnalyticsTrackingPreferences
{
    public class TrackingPreferencesModel
    {
        public AnalyticsTracking AnalyticsTracking { get; set; } = AnalyticsTracking.NotSet;
        public AnalyticsTrackingConfirmation AnalyticsTrackingConfirmation { get; set; } = AnalyticsTrackingConfirmation.ShowBanner;
    }
}
