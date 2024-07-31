using ASP.Infrastructure.Dsi.DsiApiClientProvider;
using ASP.Infrastructure.Dsi.Models;
using Newtonsoft.Json;
using System.Net;
using ASP.Core.Results;
using Microsoft.Extensions.Logging;

namespace ASP.Infrastructure.Dsi.DsiApiClient
{
    //Http client used to access the DSI public API
    //see https://github.com/DFE-Digital/login.dfe.public-api
    //Currently, only the GetUserAccess is implemented - using for getting user Roles
    public class DsiApiClient : IDsiApiClient
    {
        private readonly HttpClient _dsiApiClientProvider;
        private readonly ILogger<DsiApiClient> _logger;

        public DsiApiClient(IDsiApiClientProvider dsiApiClientProvider, ILogger<DsiApiClient> logger)
        {
            ArgumentNullException.ThrowIfNull(dsiApiClientProvider);
            ArgumentNullException.ThrowIfNull(logger);

            _dsiApiClientProvider = dsiApiClientProvider.CreateHttpClient();
            _logger = logger;
        }

        public async Task<Result<UserAccess>> GetUserAccess(string serviceId, string organisationId, string userId)
        {
            var endpoint = $"services/{serviceId}/organisations/{organisationId}/users/{userId}";
            _logger.LogInformation("Requesting user access for endpoint: {Endpoint}", endpoint);

            try
            {
                var response = await _dsiApiClientProvider.GetAsync(endpoint);

                _logger.LogDebug("Received response with status code: {StatusCode}", response.StatusCode);

                response.EnsureSuccessStatusCode();

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogInformation("User not found for endpoint: {Endpoint}", endpoint);
                    return Error.NotFound($@"Not found: User not found for endpoint: ""{endpoint}"".");
                }
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Received non-success status code {StatusCode} for endpoint: {Endpoint}", response.StatusCode, endpoint);
                    return Error.Unexpected($@"API request failed with status code ""{response.StatusCode}"" for endpoint: ""{endpoint}"".", null);
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("Received response content: {ResponseContent}", responseContent);
        
                try
                {
                    var userAccessToService = JsonConvert.DeserializeObject<UserAccess>(responseContent);
                    _logger.LogInformation("Successfully deserialized user access for endpoint: {Endpoint}", endpoint);
                    return userAccessToService!;
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Failed to deserialize the response content for endpoint: {Endpoint}", endpoint);
                    return Error.Unexpected($@"Failed to deserialize the response content. ""{ex}"".", ex.StackTrace);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed for endpoint: {Endpoint}", endpoint);
                return Error.Unexpected($@"HTTP request failed for endpoint: ""{endpoint}"", exception ""{ex}"".", ex.StackTrace);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                return Error.Unexpected(ex.Message, ex.StackTrace);
            }
        }
    }
}
