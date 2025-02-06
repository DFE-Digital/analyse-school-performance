namespace ASP.Api.Client.Establishments;

public record IsEstablishmentAccessibleInScopeRequest(string Urn, EstablishmentScopeType ScopeType, string? ScopeIdentifier);