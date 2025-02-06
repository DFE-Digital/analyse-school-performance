namespace ASP.Api.Client.Establishments;

public record EstablishmentSearchSuggestionsRequest(
    string SearchTerm,
    EstablishmentScopeType ScopeType,
    string? ScopeIdentifier,
    int? MaxSuggestions);
