namespace ASP.Infrastructure.Dsi.DsiApiClientProvider
{
    public interface IDsiApiClientProvider
    {
        HttpClient CreateHttpClient();
    }
}
