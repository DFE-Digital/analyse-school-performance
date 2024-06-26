using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ASP.Infrastructure.Dsi.DsiApiClientProvider
{
    public class SymmetricSecurityKeyProvider : ISecurityKeyProvider
    {
        private readonly DsiPublicApiConfiguration _dsiPublicApiConfiguration;

        public SymmetricSecurityKeyProvider(IOptions<DsiPublicApiConfiguration> dsiPublicApiConfiguration)
        {
            _dsiPublicApiConfiguration = dsiPublicApiConfiguration.Value ??
                throw new ArgumentNullException(nameof(dsiPublicApiConfiguration));
        }

        public SecurityKey SecurityKeyInstance => new SymmetricSecurityKey(GetEncodedDsiSecret);
        public string SecurityAlgorithm => SecurityAlgorithms.HmacSha256Signature;

        private byte[] GetEncodedDsiSecret =>
            string.IsNullOrWhiteSpace(_dsiPublicApiConfiguration.DsiApiClientSecret) ?
            throw new SecurityTokenSignatureKeyNotFoundException("Unable to locate required signing key.") :
            Encoding.ASCII.GetBytes(_dsiPublicApiConfiguration.DsiApiClientSecret);
    }
}
