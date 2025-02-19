namespace ASP.Api.Client.Schools;

public record SchoolsGetSearchSuggestionsRequest(
    string SearchTerm,
    SchoolsScopeInfo? Scope,
    int? MaxSuggestions);
