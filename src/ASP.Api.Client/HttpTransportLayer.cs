using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Client;

/// <summary>
/// Implementation of the <see cref="ITransportLayer "/> abstraction for the <see cref="AspApiClient"/> that
/// uses a <see cref="HttpClient"/> to connect to an Azure Functions app.
/// </summary>
public class HttpTransportLayer : ITransportLayer
{
    private readonly ApiOptions _options;
    private readonly ILogger<HttpTransportLayer> _logger;
    private readonly HttpClient _httpClient;

    public HttpTransportLayer(IOptions<ApiOptions> options, ILogger<HttpTransportLayer> logger)
    {
        _options = (options ?? throw new ArgumentNullException(nameof(options)))
            .Value;

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _httpClient = new HttpClient();
    }

    public Task<HttpResponseMessage> ExecuteRequest(HttpRequestMessage request)
    {
        request.RequestUri = new Uri(_options.EndpointBaseUrl + request.RequestUri?.PathAndQuery);
        request.Headers.Add("x-functions-key", _options.FunctionsKey);

        return _httpClient.SendAsync(request);
    }
}
