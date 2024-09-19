using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Infrastructure.Api
{
    /// <summary>
    /// Implementation of the <see cref="ITransportLayer "/> abstraction for the <see cref="AspApiClient"/> that
    /// uses a <see cref="System.Net.Http.HttpClient"/> to connect to an Azure Functions app.
    /// </summary>
    public class HttpTransportLayer : ITransportLayer
    {
        private readonly HttpApiOptions _options;
        private readonly ILogger<HttpTransportLayer> _logger;
        private readonly HttpClient _httpClient;

        public HttpTransportLayer(IOptions<HttpApiOptions> options, ILogger<HttpTransportLayer> logger)
        {
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _httpClient = new HttpClient { 
                BaseAddress = new Uri(_options.EndpointBaseUrl) 
            };
        }

        public async Task<TransportLayerResponse> ExecuteRequest(TransportLayerRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Parse(request.Method), request.Path + request.QueryString);
            httpRequest.Content = new StringContent(request.Body ?? "");
            httpRequest.Headers.Add("x-functions-key", _options.FunctionsKey ?? "");
            
            var httpResponse = await _httpClient.SendAsync(httpRequest);

            var response = new TransportLayerResponse();
            response.StatusCode = (int)httpResponse.StatusCode;
            response.BodyString = await httpResponse.Content.ReadAsStringAsync();
            foreach (var header in httpResponse.Headers)
            {
                response.Headers[header.Key] = header.Value.ToString() ?? "";
            }

            return response;
        }
    }
}
