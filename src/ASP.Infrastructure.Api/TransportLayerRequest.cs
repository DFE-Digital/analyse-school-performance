namespace ASP.Infrastructure.Api
{
    /// <summary>
    /// Request object for interfacing with an <see cref="ITransportLayer"/>
    /// </summary>
    public class TransportLayerRequest
    {
        public string? Method { get; set; }
        public string? Path { get; set; }
        public string? QueryString { get; set; }
        public string? Body { get; set; }
    }
}
