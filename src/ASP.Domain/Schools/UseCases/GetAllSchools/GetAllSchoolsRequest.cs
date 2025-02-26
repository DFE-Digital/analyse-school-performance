using ASP.Core.Optionality;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools.UseCases.GetAllSchools;

public record GetAllSchoolsRequest(
    Optional<SchoolAccessScopeInfo> Scope,
    Optional<int> Page,
    Optional<int> ResultsPerPage);
