namespace ASP.Infrastructure.Api
{
    /// <summary>
    /// Options object that provides configuration for the <see cref="HttpTransportLayer"/>, such as the endpoint URL
    /// and access keys.
    /// </summary>
    public class HttpApiOptions
    {
        public string EndpointBaseUrl { get; set; } = "";
        public string? FunctionsKey { get; set; }
    }
}