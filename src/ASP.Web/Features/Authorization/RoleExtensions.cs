using ASP.Api.Client.Establishments;
using ASP.Core.Authorization;
using ASP.Core.Results;
using System.Security.Claims;

namespace ASP.Web.Features.Authorization
{
    public static class RoleExtensions
    {
        public static Result<string> GetLocalAuthorityCode(this ClaimsPrincipal user)
        {
            if (!(user.Role() is Role r && r.IsLaUser))
            {
                return Result.NotAllowed<string>("User is not a Local Authority user.");
            }

            var claim = user.FindFirst(CustomClaimTypes.EstablishmentNumber);

            if (string.IsNullOrEmpty(claim?.Value))
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

        public static Result<string> GetMatUid(this ClaimsPrincipal user)
        {
            if (!(user.Role() is Role r && r.IsMatUser))
            {
                return Result.NotAllowed<string>("User is not a Multi-Academy Trust user.");
            }

            var claim = user.FindFirst(CustomClaimTypes.UniqueIdentifier);

            if (string.IsNullOrEmpty(claim?.Value))
            {
                return Result.NotAllowed<string>($"User's {CustomClaimTypes.UniqueIdentifier} claim is missing or empty.");
            }

            return claim.Value;
        }

        public static Result<string> GetDioceseName(this ClaimsPrincipal user)
        {
            if (!(user.Role() is Role r && r.IsDioceseUser))
            {
                return Result.NotAllowed<string>("User is not a Diocese user.");
            }

            var claim = user.FindFirst(CustomClaimTypes.OrganisationName);

            if (string.IsNullOrEmpty(claim?.Value))
            {
                return Result.NotAllowed<string>($"User's {CustomClaimTypes.OrganisationName} claim is missing or empty.");
            }

            return claim.Value;
        }

        public static Result<EstablishmentScopeInfo> GetScopeInfoForRole(this ClaimsPrincipal user)
        {
            if (user.Role()!.IsLaUser)
            {
                return
                    from scopeId in GetScopeIdForLaUser(user)
                    select new EstablishmentScopeInfo(EstablishmentScopeType.LA, scopeId);
            }

            if (user.Role()!.IsMatUser)
            {
                return
                    from scopeId in GetScopeIdForMatUser(user)
                    select new EstablishmentScopeInfo(EstablishmentScopeType.MAT, scopeId);
            }

            if (user.Role()!.IsDioceseUser)
            {
                return
                    from name in user.GetDioceseName()
                    select new EstablishmentScopeInfo(EstablishmentScopeType.Diocese, name);
            }

            return Result.Success(
                new EstablishmentScopeInfo(EstablishmentScopeType.All, null)
            );
        }

        private static Result<string?> GetScopeIdForLaUser(ClaimsPrincipal user)
        {
            return
                from laCode in user.GetLocalAuthorityCode()
                select laCode;
        }

        private static Result<string?> GetScopeIdForMatUser(ClaimsPrincipal user)
        {
            return
                from matUid in user.GetMatUid()
                select matUid;
        }
    }
}