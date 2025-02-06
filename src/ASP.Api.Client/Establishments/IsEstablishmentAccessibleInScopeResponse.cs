namespace ASP.Api.Client.Establishments;

public record IsEstablishmentAccessibleInScopeResponse(
    string Urn,
    string Scope,
    string ScopeIdentifier,
    bool IsAccessible);
