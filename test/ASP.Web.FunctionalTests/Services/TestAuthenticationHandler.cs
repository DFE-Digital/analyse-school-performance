using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ASP.Web.FunctionalTests.Services
{
    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationHandlerOptions>
    {
        private readonly List<Claim> _claims;

        public const string AuthenticationScheme = "Test";

        public TestAuthenticationHandler(
            IOptionsMonitor<TestAuthenticationHandlerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            TestClaimsProvider testClaimsProvider) : base(options, logger, encoder)
        {
            _claims = testClaimsProvider.GetClaims();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!_claims.Any())
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var identity = new ClaimsIdentity(_claims, AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}