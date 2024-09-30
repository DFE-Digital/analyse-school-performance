using System.Security.Claims;
using ASP.Core.Authorization;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Optionality;
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
    
    public static Task<Result<ScopeInfo>> GetScopeInfoForRole(ClaimsPrincipal user, 
        ILocalAuthorityRepository localAuthorityRepository,
        IMultiAcademyTrustRepository multiAcademyTrustRepository)
    {
        if (user.Role()!.IsLaUser)
        {
            return GetScopeIdForLaUser(user, localAuthorityRepository)
                .Map(scopeId => new ScopeInfo(ScopeType.LA, scopeId));
        }

        if (user.Role()!.IsMatUser)
        {
            return GetScopeIdForMatUser(user, multiAcademyTrustRepository)
                .Map(scopeId => new ScopeInfo(ScopeType.MAT, scopeId));
        }

        if (user.Role()!.IsDioceseUser)
        {
            return Task.FromResult(
                user.GetDioceseName()
                    .Map(name => new ScopeInfo(ScopeType.Diocese, Optional<string>.Some(name)))
            );
        }

        return Task.FromResult(Result.Success(
            new ScopeInfo(ScopeType.All, Optional<string>.None)
        ));
    }

    private static Task<Result<Optional<string>>> GetScopeIdForLaUser(ClaimsPrincipal user,
        ILocalAuthorityRepository localAuthorityRepository)
    {
        return user.GetLocalAuthorityCode()
            .Then(laCode => localAuthorityRepository.GetLocalAuthority(laCode)
                .MapError(error => error is NotFoundError ? Error.Unexpected(error.Message, null) : error)
                .Map(_ => Optional<string>.Some(laCode)));
    }

    private static Task<Result<Optional<string>>> GetScopeIdForMatUser(ClaimsPrincipal user,
        IMultiAcademyTrustRepository multiAcademyTrustRepository)
    {
        return user.GetMatUid()
            .Then(matUid => multiAcademyTrustRepository.GetMultiAcademyTrust(matUid)
                .MapError(error => error is NotFoundError ? Error.Unexpected(error.Message, null) : error)
                .Map(_ => Optional<string>.Some(matUid)));
    }
}