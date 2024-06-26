namespace ASP.Infrastructure.Constants
{
    public static class DsiConstants
    {
        //DSI public API configuration
        public const string DsiPublicApiSection = "DsiPublicApi";

        public const string DsiApiClientId = "DsiApiClientId";
        public const string DsiApiClientSecret = "DsiApiClientSecret";
        public const string DsiApiAuthorisationUrl = "DsiApiAuthorisationUrl";
        public const string DsiApiAudience = "DsiApiAudience";

        //DSI Authentication configuration
        public const string DsiSection = "DsiOidc";

        public const string DsiCookieName = "DsiCookie";

        public const string DsiOidcClientId = "DsiOidcClientId";
        public const string DsiOidcClientSecret = "DsiOidcClientSecret";
        public const string DsiAudience = "DsiAudience";
        public const string DsiIssuer = "DsiIssuer";
        public const string DsiMetadataAddress = "DsiMetadataAddress";
        public const string DsiServiceId = "DsiServiceId";

        public const string DsiScopeOpenId = "openid";
        public const string DsiScopeEmail = "email";
        public const string DsiScopeProfile = "profile";
        public const string DsiScopeOrganisationId = "organisationid";

        public const string DsiCallbackPath = "DsiCallbackPath";
        public const string DsiSignedOutCallbackPath = "DsiSignedOutCallbackPath";
        public const string DsiSignedOutRedirectUri = "DsiSignedOutRedirectUri";

        public const string AccessDeniedRoute = "/error/accessdenied";

        public const string SessionTimeout = "SessionTimeout";

        //Id token related constants
        public const string Organisation = "organisation";
        public const string NameIdentifier = "nameidentifier";
        public const string Id = "id";
        public const string AuthenticationMethod = "OpenIdConnect";
    }
}