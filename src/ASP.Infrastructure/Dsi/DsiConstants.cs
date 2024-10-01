namespace ASP.Infrastructure.Dsi
{
    public static class DsiConstants
    {
        public const string DsiCookieName = "DsiCookie";
        public const string DsiScopeOpenId = "openid";
        public const string DsiScopeEmail = "email";
        public const string DsiScopeProfile = "profile";
        public const string DsiScopeOrganisation = "organisation";
        public const string AccessDeniedRoute = "/error/accessdenied";

        //Id token related constants
        public const string Organisation = "organisation";
        public const string NameIdentifier = "nameidentifier";
        public const string Id = "id";
        public const string AuthenticationMethod = "OpenIdConnect";
    }
}