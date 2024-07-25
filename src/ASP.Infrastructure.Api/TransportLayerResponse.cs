namespace ASP.Infrastructure.Api
{
    /// <summary>
    /// Response object for interfacing with an <see cref="ITransportLayer"/>
    /// </summary>
    public class TransportLayerResponse
    {
        public int? StatusCode { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public string? Body { get; set; }
    }
}