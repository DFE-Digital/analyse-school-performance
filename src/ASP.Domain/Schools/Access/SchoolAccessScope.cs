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
        private readonly ISchoolRepository _schoolRepository;

        public Validator(
            ILocalAuthorityRepository localAuthorityRepository,
            IMultiAcademyTrustRepository multiAcademyTrustRepository, ISchoolRepository schoolRepository)
        {
            _localAuthorityRepository = localAuthorityRepository;
            _multiAcademyTrustRepository = multiAcademyTrustRepository;
            _schoolRepository = schoolRepository;
        }

        public async Task<Result<SchoolAccessScope>> ValidateScope(SchoolAccessScopeInfo scope)
        {
            if (string.IsNullOrWhiteSpace(scope.ScopeId))
            {
                return Error.Invalid($@"Scope Id must not be empty.");
            }

            if (scope.ScopeType == SchoolAccessScopeType.LA)
            {
                // Validate Scope Identifier
                return await (
                    from laCode in LACode.Parse(scope.ScopeId)
                    from _ in _localAuthorityRepository.Get(laCode)
                    select new SchoolAccessScope(scope.ScopeType, scope.ScopeId)
                ).MapErrorIf(e => e is NotFoundError, Error.Invalid($@"Local Authority with code ""{scope.ScopeId}"" does not exist."));
            }

            if (scope.ScopeType == SchoolAccessScopeType.MAT)
            {
                // Validate Scope Identifier
                return await (
                    from _ in _multiAcademyTrustRepository.GetMultiAcademyTrust(scope.ScopeId)
                    select new SchoolAccessScope(scope.ScopeType, scope.ScopeId)
                ).MapErrorIf(e => e is NotFoundError, Error.Invalid($@"Multi-Academy Trust with UID ""{scope.ScopeId}"" does not exist."));
            }

            if (scope.ScopeType == SchoolAccessScopeType.Diocese)
            {
                return new SchoolAccessScope(scope.ScopeType, scope.ScopeId);
            }

            if (scope.ScopeType == SchoolAccessScopeType.School)
            {
                // Validate Scope Identifier
                return await (
                    from urn in SchoolUrn.Parse(scope.ScopeId)
                    from school in _schoolRepository.Get(urn)
                    select new SchoolAccessScope(scope.ScopeType, scope.ScopeId)
                );
            }

            return Error.Invalid($@"Unknown scope type ""{scope.ScopeType}"".");
        }
    }
}