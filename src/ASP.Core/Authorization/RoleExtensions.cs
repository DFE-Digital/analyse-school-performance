using ASP.Core.Results;
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

        public static Result<string> GetLocalAuthorityCode(this ClaimsPrincipal user)
        {
            if(!(user.Role() is Role r && r.IsLaUser))
            {
                return Result.NotAllowed<string>("User is not a Local Authority user.");
            }

            var claim = user.FindFirst(CustomClaimTypes.EstablishmentNumber);

            if(string.IsNullOrEmpty(claim?.Value))
            {
                return Result.NotAllowed<string>($"User's {CustomClaimTypes.EstablishmentNumber} claim is missing or empty.");
            }

            return claim.Value;
        }

        public static Result<string> GetEstablishmentUrn(this ClaimsPrincipal user)
        {
            if (!(user.Role() is Role r && r.IsSchoolUser))
            {
                return Result.NotAllowed<string>("User is not a School user.");
            }

            var claim = user.FindFirst(CustomClaimTypes.UniqueReferenceNumber);

            if (string.IsNullOrEmpty(claim?.Value))
            {
                return Result.NotAllowed<string>($"User's {CustomClaimTypes.UniqueReferenceNumber} claim is missing or empty.");
            }

            return claim.Value;
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
