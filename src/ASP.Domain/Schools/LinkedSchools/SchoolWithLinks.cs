using ASP.Core.Optionality;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools.LinkedSchools
{
    public class SchoolWithLinks : School
    {
        public SchoolWithLinks(
            SchoolUrn urn,
            LAEstabCode? laEstab,
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
            Links = links.AsReadOnly();
        }

        public IReadOnlyCollection<LinkedSchoolsLink> Links { get; }

        public bool IsAccessibleInScope(Optional<SchoolAccessScope> scope) =>
            scope.Match(
                matchedScope => matchedScope.ScopeType switch
                {
                    SchoolAccessScopeType.LA => LocalAuthority != null &&
                                                LocalAuthority.Code == matchedScope.ScopeIdentifier,

                    SchoolAccessScopeType.MAT => MultiAcademyTrust != null &&
                                                 MultiAcademyTrust.Uid == matchedScope.ScopeIdentifier,

                    SchoolAccessScopeType.Diocese => Diocese != null &&
                                                     Diocese.Name == matchedScope.ScopeIdentifier,

                    SchoolAccessScopeType.School => Urn.Value == matchedScope.ScopeIdentifier,

                    _ => false
                },
                () => true);
    }
}