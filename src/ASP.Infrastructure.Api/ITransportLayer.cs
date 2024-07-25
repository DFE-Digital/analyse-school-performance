
namespace ASP.Infrastructure.Api
{
    /// <summary>
    /// Interface for the abstraction of a transport layer to communicate with the ASP API. Implementations could include
    /// a real HTTP connection (<see cref="HttpTransportLayer"/>), an in-process connection to the API function objects
    /// in memory (<see cref="InProcessTransportLayer"/>), or a test transport layer that simulates connection issues
    /// to the API.
    /// </summary>
    public interface ITransportLayer
    {
        Task<TransportLayerResponse> ExecuteRequest(TransportLayerRequest request);
    }
}