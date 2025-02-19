namespace ASP.Api.Client.Schools;

public record SchoolsGetAccessRequest(
    string Urn,
    SchoolsScopeInfo? Scope);