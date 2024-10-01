namespace ASP.Infrastructure.Dsi
{
    public class DsiOidcOptions
    {
        public const string SectionName = "DsiOidc";

        public double SessionTimeout { get; set; } = 20.0;
        public string Audience { get; set; } = "";
        public string Issuer { get; set; } = "";
        public string MetadataAddress { get; set; } = "";
        public string ServiceId { get; set; } = "";
        public string ProfileUrl { get; set; } = "";
        public string ClientId { get; set; } = "";
        public string ClientSecret { get; set; } = "";
        public string CallbackPath { get; set; } = "";
        public string SignedOutCallbackPath { get; set; } = "";
        public string SignedOutRedirectUri { get; set; } = "";
    }
}