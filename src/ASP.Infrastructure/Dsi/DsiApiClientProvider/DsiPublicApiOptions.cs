namespace ASP.Infrastructure.Dsi.DsiApiClientProvider;

public class DsiPublicApiOptions
{
    public const string SectionName = "DsiPublicApi";

    public string AuthorizationUrl { get; set; } = "";
    public string Audience { get; set; } = "";
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
}
