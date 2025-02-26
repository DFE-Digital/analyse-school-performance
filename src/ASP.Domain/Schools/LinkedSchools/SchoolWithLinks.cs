using ASP.Core.Optionality;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools.LinkedSchools
{
    public class SchoolWithLinks : School
    {
        public SchoolWithLinks(
            SchoolUrn urn,
            LAEstabCode laEstab,
            string name,
            EducationPhase educationPhase,
            Address? address,
            DateTime? openDate,
            DateTime? closeDate,
            LocalAuthority? localAuthority,
            MultiAcademyTrust? multiAcademyTrust,
            Diocese? diocese,
            List<LinkedSchoolsLink> links)
            : base(urn, laEstab, name, educationPhase, address, openDate, closeDate, localAuthority, multiAcademyTrust, diocese)
        {
            Links = links;
        }

        public List<LinkedSchoolsLink> Links { get; }
        public List<SchoolUrn> LinkedUrns => Links
            .SelectMany(l => l.LinkedSchools)
            .Select(l => l.Urn)
            .Distinct()
            .ToList();

        public SchoolAccess GetAccessForScope(Optional<SchoolAccessScope> scope)
        {
            return new SchoolAccess(
                IsAccessibleInScope(scope),
                IsAccessibleViaLinkedSchools(scope));
        }

        private bool IsAccessibleInScope(Optional<SchoolAccessScope> scope) =>
            scope.Match(
                scope => scope.ScopeType switch {
                    SchoolAccessScopeType.LA => LocalAuthority != null &&
                        LocalAuthority.Code.ToString() == scope.ScopeIdentifier,

                    SchoolAccessScopeType.MAT => MultiAcademyTrust != null &&
                        MultiAcademyTrust.Uid.ToString() == scope.ScopeIdentifier,

                    SchoolAccessScopeType.Diocese => Diocese != null &&
                        Diocese.Name == scope.ScopeIdentifier,

                    _ => false // Default case returns false for any unhandled ScopeType
                },
                () => true);

        private bool IsAccessibleViaLinkedSchools(Optional<SchoolAccessScope> scope) =>
            scope.Match(
                scope =>
                {
                    if (scope.ScopeType is SchoolAccessScopeType.LA)
                    {
                        return false;
                    }

                    if (!Links.Any())
                    {
                        return false;
                    }

                    return scope.ScopeType switch {
                        SchoolAccessScopeType.MAT => Links.Any(
                            link => link.LinkedSchools.Any(school => school.MultiAcademyTrust?.Uid.ToString() == scope.ScopeIdentifier)),

                        SchoolAccessScopeType.Diocese => Links.Any(
                            link => link.LinkedSchools.Any(school => school.Diocese?.Name == scope.ScopeIdentifier)),

                        _ => false
                    };
                },
                () => false);
    }
}
