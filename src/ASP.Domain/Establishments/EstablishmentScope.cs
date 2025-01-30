using System.Security.Claims;
using ASP.Core.Authorization;
using ASP.Domain.LocalAuthorities;
using ASP.Domain.MultiAcademyTrusts;
using ASP.Core.Optionality;
using ASP.Core.Results;

namespace ASP.Domain.Establishments;

public class EstablishmentScope
{
    public EstablishmentScopeType ScopeType { get; }
    public string ScopeIdentifier { get; }

    public EstablishmentScope(EstablishmentScopeType scopeType, string scopeIdentifier)
    {
        ScopeType = scopeType;
        ScopeIdentifier = scopeIdentifier;
    }

    public class Validator : IEstablishmentScopeValidator
    {
        private readonly ILocalAuthorityRepository _localAuthorityRepository;
        private readonly IMultiAcademyTrustRepository _multiAcademyTrustRepository;

        public Validator(
            ILocalAuthorityRepository localAuthorityRepository,
            IMultiAcademyTrustRepository multiAcademyTrustRepository
        )
        {
            _localAuthorityRepository = localAuthorityRepository;
            _multiAcademyTrustRepository = multiAcademyTrustRepository;
        }

        public async Task<Result<EstablishmentScope>> ValidateScope(EstablishmentScopeType scopeType, string? scopeIdentifier)
        {
            if (scopeType == EstablishmentScopeType.LA)
            {
                if (scopeIdentifier is null || string.IsNullOrWhiteSpace(scopeIdentifier))
                {
                    return Error.Invalid($@"ScopeIdentifier for LA scope must not be empty.");
                }

                // Validate Scope Identifier
                return
                    from _ in await _localAuthorityRepository.GetLocalAuthority(scopeIdentifier)
                        .MapError(e => e is NotFoundError
                            ? Error.Invalid($@"Local Authority with code ""{scopeIdentifier}"" does not exist.")
                            : e)
                    select new EstablishmentScope(scopeType, scopeIdentifier);
            }

            if (scopeType == EstablishmentScopeType.MAT)
            {
                if (scopeIdentifier is null || string.IsNullOrWhiteSpace(scopeIdentifier))
                {
                    return Error.Invalid($@"ScopeIdentifier for MAT scope must not be empty.");
                }

                // Validate Scope Identifier
                return
                    from _ in await _multiAcademyTrustRepository.GetMultiAcademyTrust(scopeIdentifier)
                        .MapError(e => e is NotFoundError
                            ? Error.Invalid($@"Multi-Academy Trust with UID ""{scopeIdentifier}"" does not exist.")
                            : e)
                    select new EstablishmentScope(scopeType, scopeIdentifier);
            }

            if (scopeType == EstablishmentScopeType.Diocese)
            {
                if (scopeIdentifier is null || string.IsNullOrWhiteSpace(scopeIdentifier))
                {
                    return Error.Invalid($@"ScopeIdentifier for Diocese scope must not be empty.");
                }

                return new EstablishmentScope(scopeType, scopeIdentifier);
            }

            return new EstablishmentScope(scopeType, "");
        }

        public Task<Result<EstablishmentScopeInfo>> GetScopeInfoForRole(ClaimsPrincipal user)
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
                return Task.FromResult(
                    from name in user.GetDioceseName()
                    let scopeId = Optional<string>.Some(name)
                    select new EstablishmentScopeInfo(EstablishmentScopeType.Diocese, scopeId)
                );
            }

            return Task.FromResult(Result.Success(
                new EstablishmentScopeInfo(EstablishmentScopeType.All, Optional<string>.None)
            ));
        }

        private Task<Result<Optional<string>>> GetScopeIdForLaUser(ClaimsPrincipal user)
        {
            return
                from laCode in user.GetLocalAuthorityCode()
                from _ in _localAuthorityRepository.GetLocalAuthority(laCode)
                    .MapError(error => error is NotFoundError ? Error.Unexpected(error.Message, null) : error)
                select Optional<string>.Some(laCode);
        }

        private Task<Result<Optional<string>>> GetScopeIdForMatUser(ClaimsPrincipal user)
        {
            return
                from matUid in user.GetMatUid()
                from _ in _multiAcademyTrustRepository.GetMultiAcademyTrust(matUid)
                    .MapError(error => error is NotFoundError ? Error.Unexpected(error.Message, null) : error)
                select Optional<string>.Some(matUid);
        }
    }
}