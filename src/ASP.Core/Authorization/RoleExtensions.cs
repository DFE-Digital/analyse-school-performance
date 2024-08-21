using Microsoft.AspNetCore.Authorization;
using System.Data;
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
            return Authorization.Role.FromClaimsPrincipal(user) is Role r && r == role;
        }

        public static bool HasRole(this ClaimsPrincipal user, Func<Role, bool> roleCondition)
        {
            return Authorization.Role.FromClaimsPrincipal(user) is Role r && roleCondition(r);
        }

        public static AuthorizationPolicyBuilder RequireRole(this AuthorizationPolicyBuilder policy, Role role)
        {
            policy.RequireRole(role.Code);
            return policy;
        }

        public static AuthorizationPolicyBuilder RequireRole(this AuthorizationPolicyBuilder policy, RoleCollection roles)
        {
            policy.RequireRole(roles.Select(r => r.Code));
            return policy;
        }
    }
}
