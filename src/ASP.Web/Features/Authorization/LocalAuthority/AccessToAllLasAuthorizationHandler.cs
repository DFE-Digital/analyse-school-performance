using ASP.Core.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace ASP.Web.Features.Authorization.LocalAuthority;

public class AccessToAllLasAuthorizationHandler : AuthorizationHandler<LocalAuthorityRequirement>
{
    private readonly ILogger<AccessToAllLasAuthorizationHandler> _logger;

    public AccessToAllLasAuthorizationHandler(ILogger<AccessToAllLasAuthorizationHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        LocalAuthorityRequirement requirement
    )
    {
        var user = context.User;

        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            _logger.LogWarning("User is not authenticated.");
            return Task.CompletedTask;
        }

        var userRole = Role.FromClaimsPrincipal(user);
        if(userRole is null)
        {
            _logger.LogWarning("User has no roles.");
            return Task.CompletedTask;
        }

        if (userRole.HasAccessToAllLas)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}