using ASP.Core.Results;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ASP.Core.Authorization
{
    public static class RoleExtensions
    {
        public static Role? Role(this ClaimsPrincipal user)
        {
            return Authorization.Role.FromClaimsPrincipal(user);
        }

        public static bool HasRole(this ClaimsPrincipal user, Role role)
        {
            return user.Role() is Role r && r == role;
        }

        public static bool HasRole(this ClaimsPrincipal user, Func<Role, bool> roleCondition)
        {
            return user.Role() is Role r && roleCondition(r);
        }

        public static bool HasRole(this ClaimsPrincipal user, RoleCollection roles)
        {
            return user.Role() is Role r && roles.Contains(r);
        }

        public static AuthorizationPolicyBuilder RequireRole(this AuthorizationPolicyBuilder policy, Role role)
        {
            return policy.RequireRole(role.Code);
        }

        public static AuthorizationPolicyBuilder RequireRole(this AuthorizationPolicyBuilder policy, RoleCollection roles)
        {
            return policy.RequireRole(roles.Select(r => r.Code));
        }
    }
}
