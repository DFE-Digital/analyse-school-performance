using System.Security.Claims;
using ASP.Core.Authorisation;
using ASP.Core.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace ASP.Web.Features.Authorisation.LocalAuthority;

public class AccessToAllLasAuthorizationHandler : AuthorizationHandler<LocalAuthorityRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        LocalAuthorityRequirement requirement)
    {
        var user = context.User;

        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return Task.CompletedTask;
        }

        var userRole = ClaimsHelper.GetFirstNonEmptyRoleClaim(user);

        if (Roles.AccessToAllLas.Contains(userRole))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}