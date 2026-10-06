This component represents the "Set your analytics preferences" form that appears in the Cookies page:

![image.png](/.attachments/image-298375d7-1560-4f79-bba2-2a9cd563f156.png)

The component is contained within `ASP.Web\Features\AnalyticsTrackingPreferences`, and submits to the `SavePreferences()` action of the `AnalyticsTrackingPreferencesController`.

The JSON structure of the component is as follows:

```
{
  "ViewId": "CookiePreferences"
}
```