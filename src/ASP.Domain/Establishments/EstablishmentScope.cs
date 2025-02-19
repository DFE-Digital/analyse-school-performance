using ASP.Domain.LocalAuthorities;
using ASP.Domain.MultiAcademyTrusts;
using ASP.Core.Results;
using ASP.Core.Optionality;

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

        public Task<Result<EstablishmentScope>> ValidateScope(Optional<EstablishmentScopeInfo> scope)
        {
            return scope.Match(
                s => ValidateScope(s.ScopeType, s.ScopeId),
                () => ValidateScope(EstablishmentScopeType.All, ""));
        }
    }
}