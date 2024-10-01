namespace ASP.Infrastructure.Api
{
    /// <summary>
    /// Options object that provides configuration for the <see cref="HttpTransportLayer"/>, such as the endpoint URL
    /// and access keys.
    /// </summary>
    public class ApiOptions
    {
        public const string SectionName = "Api";

        public bool InProcess { get; set; } = false;
        public string EndpointBaseUrl { get; set; } = "";
        public string FunctionsKey { get; set; } = "";
    }
}