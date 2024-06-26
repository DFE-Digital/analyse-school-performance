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
        private readonly DsiPublicApiConfiguration _dsiPublicApiConfiguration;
        private readonly ISecurityKeyProvider _securityKeyProvider;

        public DsiApiClientProvider(
            HttpClient httpClient,
            IOptions<DsiPublicApiConfiguration> dsiPublicApiConfiguration,
            ISecurityKeyProvider securityKeyProvider)
        {
            _httpClient = httpClient ??
                throw new ArgumentNullException(nameof(httpClient));
            _dsiPublicApiConfiguration = dsiPublicApiConfiguration.Value ??
                throw new ArgumentNullException(nameof(dsiPublicApiConfiguration));
            _securityKeyProvider = securityKeyProvider ??
                throw new ArgumentNullException(nameof(securityKeyProvider));
        }

        public HttpClient CreateHttpClient()
        {
            const string TokenMediaType = "application/json";
            const string TokenScheme = "Bearer";

            string encodedDsiAccessToken = CreateEncodedDsiAccessToken();

            string dsiAuthorisationUrl = _dsiPublicApiConfiguration.DsiApiAuthorisationUrl!.TrimEnd('/');

            _httpClient.BaseAddress = new Uri(dsiAuthorisationUrl);

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
                        Issuer = _dsiPublicApiConfiguration.DsiApiClientId,
                        Audience = _dsiPublicApiConfiguration.DsiApiAudience,
                        SigningCredentials =
                            new SigningCredentials(
                                _securityKeyProvider.SecurityKeyInstance,
                                _securityKeyProvider.SecurityAlgorithm)
                    });
    }
}
