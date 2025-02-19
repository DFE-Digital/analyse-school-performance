namespace ASP.Api.Client.Schools;

public record SchoolsGetAllRequest(
    string? SearchTerm,
    SchoolsScopeInfo? Scope,
    int? Page,
    int? ResultsPerPage);