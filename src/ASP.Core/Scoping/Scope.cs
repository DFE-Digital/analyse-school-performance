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

            // Validate Scope Identifier
            return
                from _ in await localAuthorityRepository.GetLocalAuthority(scopeIdentifier)
                    .MapError(e => e is NotFoundError
                        ? Error.Invalid($@"Local Authority with code ""{scopeIdentifier}"" does not exist.")
                        : e)
                select new Scope(scopeType, scopeIdentifier);
        }

        if (scopeType == ScopeType.MAT)
        {
            if (scopeIdentifier is null || string.IsNullOrWhiteSpace(scopeIdentifier))
            {
                return Error.Invalid($@"ScopeIdentifier for MAT scope must not be empty.");
            }

            // Validate Scope Identifier
            return
                from _ in await multiAcademyTrustRepository.GetMultiAcademyTrust(scopeIdentifier)
                    .MapError(e => e is NotFoundError
                        ? Error.Invalid($@"Multi-Academy Trust with UID ""{scopeIdentifier}"" does not exist.")
                        : e)
                select new Scope(scopeType, scopeIdentifier);
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
            return 
                from scopeId in GetScopeIdForLaUser(user, localAuthorityRepository)
                select new ScopeInfo(ScopeType.LA, scopeId);
        }

        if (user.Role()!.IsMatUser)
        {
            return 
                from scopeId in GetScopeIdForMatUser(user, multiAcademyTrustRepository)
                select new ScopeInfo(ScopeType.MAT, scopeId);
        }

        if (user.Role()!.IsDioceseUser)
        {
            return Task.FromResult(
                from name in user.GetDioceseName()
                let scopeId = Optional<string>.Some(name)
                select new ScopeInfo(ScopeType.Diocese, scopeId)
            );
        }

        return Task.FromResult(Result.Success(
            new ScopeInfo(ScopeType.All, Optional<string>.None)
        ));
    }

    private static Task<Result<Optional<string>>> GetScopeIdForLaUser(ClaimsPrincipal user,
        ILocalAuthorityRepository localAuthorityRepository)
    {
        return
            from laCode in user.GetLocalAuthorityCode()
            from _ in localAuthorityRepository.GetLocalAuthority(laCode)
                .MapError(error => error is NotFoundError ? Error.Unexpected(error.Message, null) : error)
            select Optional<string>.Some(laCode);
    }

    private static Task<Result<Optional<string>>> GetScopeIdForMatUser(ClaimsPrincipal user,
        IMultiAcademyTrustRepository multiAcademyTrustRepository)
    {
        return 
            from matUid in user.GetMatUid()
            from _ in multiAcademyTrustRepository.GetMultiAcademyTrust(matUid)
                .MapError(error => error is NotFoundError ? Error.Unexpected(error.Message, null) : error)
            select Optional<string>.Some(matUid);
    }
}