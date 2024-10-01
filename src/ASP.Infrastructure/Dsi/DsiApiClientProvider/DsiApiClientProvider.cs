using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace ASP.Infrastructure.Dsi.DsiApiClientProvider
{
    //This class creates the DSI publix API client and creates and adds the Bearer token to the request.
    public class DsiApiClientProvider : IDsiApiClientProvider
    {
        private readonly HttpClient _httpClient;
        private readonly DsiPublicApiOptions _options;
        private readonly ISecurityKeyProvider _securityKeyProvider;

        public DsiApiClientProvider(
            HttpClient httpClient,
            IOptions<DsiPublicApiOptions> options,
            ISecurityKeyProvider securityKeyProvider
        )
        {
            _httpClient = httpClient ??
                throw new ArgumentNullException(nameof(httpClient));
            _options = options?.Value ??
                throw new ArgumentNullException(nameof(options));
            _securityKeyProvider = securityKeyProvider ??
                throw new ArgumentNullException(nameof(securityKeyProvider));
        }

        public HttpClient CreateHttpClient()
        {
            const string TokenMediaType = "application/json";
            const string TokenScheme = "Bearer";

            string encodedDsiAccessToken = CreateEncodedDsiAccessToken();

            string dsiAuthorizationUrl = _options.AuthorizationUrl.TrimEnd('/');

            _httpClient.BaseAddress = new Uri(dsiAuthorizationUrl);

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(TokenMediaType));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TokenScheme, encodedDsiAccessToken);

            return _httpClient;
        }

        private string CreateEncodedDsiAccessToken() =>
            new JwtSecurityTokenHandler()
                .CreateEncodedJwt(
                    new SecurityTokenDescriptor
                    {
                        Issuer = _options.ClientId,
                        Audience = _options.Audience,
                        SigningCredentials =
                            new SigningCredentials(
                                _securityKeyProvider.SecurityKeyInstance,
                                _securityKeyProvider.SecurityAlgorithm)
                    });
    }
}
