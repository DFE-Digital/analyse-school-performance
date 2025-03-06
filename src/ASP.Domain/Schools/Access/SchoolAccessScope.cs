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
            IMultiAcademyTrustRepository multiAcademyTrustRepository)
        {
            _localAuthorityRepository = localAuthorityRepository;
            _multiAcademyTrustRepository = multiAcademyTrustRepository;
        }

        public async Task<Result<SchoolAccessScope>> ValidateScope(SchoolAccessScopeInfo scopeInfo)
        {
            if (scopeInfo.ScopeId is null || string.IsNullOrWhiteSpace(scopeInfo.ScopeId))
            {
                return Error.Invalid($@"Scope Id must not be empty.");
            }

            if (scopeInfo.ScopeType == SchoolAccessScopeType.LA)
            {
                // Validate Scope Identifier
                return await (
                    from laCode in LACode.Parse(scopeInfo.ScopeId)
                    from _ in _localAuthorityRepository.Get(laCode)
                    select new SchoolAccessScope(scopeInfo.ScopeType, scopeInfo.ScopeId)
                ).MapErrorIf(e => e is NotFoundError, Error.Invalid($@"Local Authority with code ""{scopeInfo.ScopeId}"" does not exist."));
            }

            if (scopeInfo.ScopeType == SchoolAccessScopeType.MAT)
            {
                // Validate Scope Identifier
                return await (
                    from _ in _multiAcademyTrustRepository.GetMultiAcademyTrust(scopeInfo.ScopeId)
                    select new SchoolAccessScope(scopeInfo.ScopeType, scopeInfo.ScopeId)
                ).MapErrorIf(e => e is NotFoundError, Error.Invalid($@"Multi-Academy Trust with UID ""{scopeInfo.ScopeId}"" does not exist."));
            }

            if (scopeInfo.ScopeType == SchoolAccessScopeType.Diocese)
            {
                return new SchoolAccessScope(scopeInfo.ScopeType, scopeInfo.ScopeId);
            }

            return Error.Invalid($@"Unknown scope type ""{scopeInfo.ScopeType}"".");
        }
    }
}