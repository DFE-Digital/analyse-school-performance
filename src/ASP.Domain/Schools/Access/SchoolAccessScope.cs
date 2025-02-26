using ASP.Domain.LocalAuthorities;
using ASP.Domain.MultiAcademyTrusts;
using ASP.Core.Results;

namespace ASP.Domain.Schools.Access;

public class SchoolAccessScope
{
    public SchoolAccessScopeType ScopeType { get; }
    public string ScopeIdentifier { get; }

    public SchoolAccessScope(SchoolAccessScopeType scopeType, string scopeIdentifier)
    {
        ScopeType = scopeType;
        ScopeIdentifier = scopeIdentifier;
    }

    public class Validator : ISchoolAccessScopeValidator
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

        public async Task<Result<SchoolAccessScope>> ValidateScope(SchoolAccessScopeInfo scopeInfo)
        {
            if (scopeInfo.ScopeType == SchoolAccessScopeType.LA)
            {
                if (scopeInfo.ScopeId is null || string.IsNullOrWhiteSpace(scopeInfo.ScopeId))
                {
                    return Error.Invalid($@"Scope Id for LA scope must not be empty.");
                }

                // Validate Scope Identifier
                return
                    from _ in await _localAuthorityRepository.GetLocalAuthority(scopeInfo.ScopeId)
                        .MapErrorIf(e => e is NotFoundError, Error.Invalid($@"Local Authority with code ""{scopeInfo.ScopeId}"" does not exist."))
                    select new SchoolAccessScope(scopeInfo.ScopeType, scopeInfo.ScopeId);
            }

            if (scopeInfo.ScopeType == SchoolAccessScopeType.MAT)
            {
                if (scopeInfo.ScopeId is null || string.IsNullOrWhiteSpace(scopeInfo.ScopeId))
                {
                    return Error.Invalid($@"Scope Id for MAT scope must not be empty.");
                }

                // Validate Scope Identifier
                return
                    from _ in await _multiAcademyTrustRepository.GetMultiAcademyTrust(scopeInfo.ScopeId)
                        .MapErrorIf(e => e is NotFoundError, Error.Invalid($@"Multi-Academy Trust with UID ""{scopeInfo.ScopeId}"" does not exist."))
                    select new SchoolAccessScope(scopeInfo.ScopeType, scopeInfo.ScopeId);
            }

            if (scopeInfo.ScopeType == SchoolAccessScopeType.Diocese)
            {
                if (scopeInfo.ScopeId is null || string.IsNullOrWhiteSpace(scopeInfo.ScopeId))
                {
                    return Error.Invalid($@"Scope Id for Diocese scope must not be empty.");
                }

                return new SchoolAccessScope(scopeInfo.ScopeType, scopeInfo.ScopeId);
            }

            return Result.Invalid<SchoolAccessScope>($@"Unknown scope type ""{scopeInfo.ScopeType}"".");
        }
    }
}