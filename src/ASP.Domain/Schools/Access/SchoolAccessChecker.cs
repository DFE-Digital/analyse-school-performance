using ASP.Core.Optionality;
using ASP.Domain.Schools.LinkedSchools;

namespace ASP.Domain.Schools.Access;

public class SchoolAccessChecker
{
    private readonly SchoolWithLinks _school;
    private readonly IReadOnlyCollection<string> _linkedUrns;
    private readonly IReadOnlyCollection<School> _linkedSchools;

    public SchoolAccessChecker(
        SchoolWithLinks school,
        List<string> linkedUrns,
        List<School> linkedSchools)
    {
        _school = school;
        _linkedUrns = linkedUrns.AsReadOnly();
        _linkedSchools = linkedSchools.AsReadOnly();
    }

    public SchoolAccess GetAccessForScope(Optional<SchoolAccessScope> scope)
    {
        return new SchoolAccess(
            _school.IsAccessibleInScope(scope),
            IsAccessibleViaLinkedSchools(scope));
    }

    private bool IsAccessibleViaLinkedSchools(Optional<SchoolAccessScope> scope) =>
        scope.Match(
            matchedScope =>
            {
                if (matchedScope.ScopeType is SchoolAccessScopeType.LA)
                {
                    return false;
                }

                if (!_linkedUrns.Any())
                {
                    return false;
                }

                return matchedScope.ScopeType switch
                {
                    SchoolAccessScopeType.MAT => _linkedSchools.Any(
                        school => school.MultiAcademyTrust?.Uid.ToString() == matchedScope.ScopeIdentifier),

                    SchoolAccessScopeType.Diocese => _linkedSchools.Any(
                        school => school.Diocese?.Name == matchedScope.ScopeIdentifier),

                    SchoolAccessScopeType.School => _linkedUrns.Contains(matchedScope.ScopeIdentifier),

                    _ => false
                };
            },
            () => false);
}