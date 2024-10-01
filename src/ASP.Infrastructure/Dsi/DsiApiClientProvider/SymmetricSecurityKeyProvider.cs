using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ASP.Infrastructure.Dsi.DsiApiClientProvider
{
    public class SymmetricSecurityKeyProvider : ISecurityKeyProvider
    {
        private readonly DsiPublicApiOptions _options;

        public SymmetricSecurityKeyProvider(IOptions<DsiPublicApiOptions> dsiPublicApiConfiguration)
        {
            _options = dsiPublicApiConfiguration.Value ??
                throw new ArgumentNullException(nameof(dsiPublicApiConfiguration));
        }

        public SecurityKey SecurityKeyInstance => new SymmetricSecurityKey(GetEncodedDsiSecret);
        public string SecurityAlgorithm => SecurityAlgorithms.HmacSha256Signature;

        private byte[] GetEncodedDsiSecret =>
            string.IsNullOrWhiteSpace(_options.ClientSecret) ?
            throw new SecurityTokenSignatureKeyNotFoundException("Unable to locate required signing key.") :
            Encoding.ASCII.GetBytes(_options.ClientSecret);
    }
}
