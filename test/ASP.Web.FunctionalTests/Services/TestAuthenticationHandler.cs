using ASP.Web.AcceptanceTests.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ASP.Web.FunctionalTests.Services
{
    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationHandlerOptions>
    {
        private readonly List<Claim> _roles;

        public const string AuthenticationScheme = "Test";

        public TestAuthenticationHandler(
            IOptionsMonitor<TestAuthenticationHandlerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            TestRoleProvider testRoleProvider) : base(options, logger, encoder)
        {
            _roles = testRoleProvider.GetRoles();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var identity = new ClaimsIdentity(_roles, AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
