using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Results;

namespace ASP.Core.Scope;

public class Scope
{
    public ScopeType ScopeType { get; }
    public string ScopeIdentifier { get; }

    public Scope(ScopeType scopeType, string scopeIdentifier)
    {
        ScopeType = scopeType;
        ScopeIdentifier = scopeIdentifier;
    }
    
    public async Task<Result<Done>> Validate(
        ILocalAuthorityRepository localAuthorityRepository,
        IMultiAcademyTrustRepository multiAcademyTrustRepository)
    {
        if (ScopeType == ScopeType.LA)
        {
            var laScopeInvalidError =
                Error.Invalid($@"Local Authority with code ""{ScopeIdentifier}"" does not exist.");

            // Validate Scope Identifier
            var localAuthority = await localAuthorityRepository.GetLocalAuthority(ScopeIdentifier)
                .MapError(e => e is NotFoundError
                    ? laScopeInvalidError
                    : e).GetValueOrDefault(new ASP.Core.LocalAuthorities.LocalAuthority("", ""));

            if (string.IsNullOrEmpty(localAuthority.Code))
            {
                return laScopeInvalidError;
            }
        }

        if (ScopeType == ScopeType.MAT)
        {
            var matScopeInvalidError =
                Error.Invalid($@"Multi-Academy Trust with UID ""{ScopeIdentifier}"" does not exist.");

            // Validate Scope Identifier
            var mat = await multiAcademyTrustRepository.GetMultiAcademyTrust(ScopeIdentifier)
                .MapError(e => e is NotFoundError
                    ? matScopeInvalidError
                    : e).GetValueOrDefault(
                    new MultiAcademyTrust("", ""));

            if (string.IsNullOrEmpty(mat.Id))
            {
                return matScopeInvalidError;
            }
        }

        return Result.Done;
    }
}