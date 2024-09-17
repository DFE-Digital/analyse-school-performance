using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Results;

namespace ASP.Core.Scoping;

public class Scope
{
    public ScopeType ScopeType { get; }
    public string ScopeIdentifier { get; }

    public Scope(ScopeType scopeType, string scopeIdentifier)
    {
        ScopeType = scopeType;
        ScopeIdentifier = scopeIdentifier;
    }

    public static async Task<Result<Scope>> Validate(
        ScopeType scopeType,
        string? scopeIdentifier,
        ILocalAuthorityRepository localAuthorityRepository,
        IMultiAcademyTrustRepository multiAcademyTrustRepository
    )
    {
        if (scopeType == ScopeType.LA)
        {
            if (scopeIdentifier is null || string.IsNullOrWhiteSpace(scopeIdentifier))
            {
                return Error.Invalid($@"ScopeIdentifier for LA scope must not be empty.");
            }

            var laScopeInvalidError =
                Error.Invalid($@"Local Authority with code ""{scopeIdentifier}"" does not exist.");

            // Validate Scope Identifier
            var localAuthority = await localAuthorityRepository.GetLocalAuthority(scopeIdentifier)
                .MapError(e => e is NotFoundError
                    ? laScopeInvalidError
                    : e).GetValueOrDefault(new LocalAuthority("", ""));

            if (string.IsNullOrEmpty(localAuthority.Code))
            {
                return laScopeInvalidError;
            }

            return new Scope(scopeType, scopeIdentifier);
        }

        if (scopeType == ScopeType.MAT)
        {
            if (scopeIdentifier is null || string.IsNullOrWhiteSpace(scopeIdentifier))
            {
                return Error.Invalid($@"ScopeIdentifier for MAT scope must not be empty.");
            }

            var matScopeInvalidError =
                Error.Invalid($@"Multi-Academy Trust with UID ""{scopeIdentifier}"" does not exist.");

            // Validate Scope Identifier
            var mat = await multiAcademyTrustRepository.GetMultiAcademyTrust(scopeIdentifier)
                .MapError(e => e is NotFoundError
                    ? matScopeInvalidError
                    : e).GetValueOrDefault(
                    new MultiAcademyTrust("", ""));

            if (string.IsNullOrEmpty(mat.Id))
            {
                return matScopeInvalidError;
            }

            return new Scope(scopeType, scopeIdentifier);
        }

        if (scopeType == ScopeType.Diocese)
        {
            if (scopeIdentifier is null || string.IsNullOrWhiteSpace(scopeIdentifier))
            {
                return Error.Invalid($@"ScopeIdentifier for Diocese scope must not be empty.");
            }

            return new Scope(scopeType, scopeIdentifier);
        }

        return new Scope(scopeType, "");
    }
}