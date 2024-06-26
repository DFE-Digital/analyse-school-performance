using Microsoft.IdentityModel.Tokens;

namespace ASP.Infrastructure.Dsi.DsiApiClientProvider
{
    public interface ISecurityKeyProvider
    {
        SecurityKey SecurityKeyInstance { get; }
        string SecurityAlgorithm { get; }
    }
}
