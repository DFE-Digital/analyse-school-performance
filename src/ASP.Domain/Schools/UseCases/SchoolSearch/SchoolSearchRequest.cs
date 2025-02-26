using ASP.Core.Optionality;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools.UseCases.SchoolSearch;

public record SchoolSearchRequest(
    string SearchTerm,
    Optional<SchoolAccessScopeInfo> Scope,
    Optional<int> Page,
    Optional<int> ResultsPerPage
);