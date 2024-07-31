using System.Security.Claims;
using ASP.Core.Authorisation;
using ASP.Core.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace ASP.Web.Features.Authorisation.LocalAuthority;

public class LaUserAuthorizationHandler : AuthorizationHandler<LocalAuthorityRequirement>
{
    private readonly ILogger<LaUserAuthorizationHandler> _logger;

    public LaUserAuthorizationHandler(ILogger<LaUserAuthorizationHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        LocalAuthorityRequirement requirement)
    {
        var user = context.User;

        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return Task.CompletedTask;
        }

        var userRole = ClaimsHelper.GetFirstNonEmptyRoleClaim(user);
        
        var userLaCode = user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.EstablishmentNumber)?.Value;

        string? laCode = null;
        if (context.Resource is HttpContext httpContext)
        {
            laCode = httpContext.Request.RouteValues["laCode"] as string;
        }
        else
        {
            _logger.LogWarning("Expected HttpContext but got {ResourceType}", context.Resource?.GetType().Name ?? "null");
            return Task.CompletedTask;
        }

        if (string.IsNullOrEmpty(userLaCode))
        {
            _logger.LogWarning("User LA code is null or empty for user with role {UserRole}", userRole);
            return Task.CompletedTask;
        }

        if (string.IsNullOrEmpty(laCode))
        {
            _logger.LogWarning("LA code from route is null or empty");
            return Task.CompletedTask;
        }

        if ((userRole == Roles.LaNamed || userRole == Roles.LaUnnamed) && userLaCode == laCode)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}