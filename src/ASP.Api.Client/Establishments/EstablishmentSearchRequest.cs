namespace ASP.Api.Client.Establishments;

public record EstablishmentSearchRequest(
    string SearchTerm,
    EstablishmentScopeType ScopeType,
    string? ScopeIdentifier,
    int? Page,
    int? ResultsPerPage);