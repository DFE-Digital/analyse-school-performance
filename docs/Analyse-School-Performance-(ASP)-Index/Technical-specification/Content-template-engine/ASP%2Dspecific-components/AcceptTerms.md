This component represents the "Continue" button on the Accept Terms of Use page that appears blocking the user from continuing to the application until they click the button:

![image.png](/.attachments/image-773f6a70-2045-40c7-908d-1d87fe9165ea.png)

The component is contained within `ASP.Web\Features\TermsOfUse`, and submits to the `Accept()` action of the `TermsOfUseController`.

The JSON structure of the component is as follows:

```
{
  "ViewId": "AcceptTerms"
}
```