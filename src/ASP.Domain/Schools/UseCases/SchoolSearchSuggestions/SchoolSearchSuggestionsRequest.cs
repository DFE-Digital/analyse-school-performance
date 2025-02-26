using ASP.Core.Optionality;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools.UseCases.SchoolSearchSuggestions;

public record SchoolSearchSuggestionsRequest(
    string SearchTerm,
    Optional<SchoolAccessScopeInfo> Scope,
    Optional<int> MaxSuggestions
);