using ASP.Core.Optionality;

namespace ASP.Domain.Establishments.UseCases.EstablishmentSearchSuggestions;

public record EstablishmentSearchSuggestionsRequest(
    string SearchTerm,
    EstablishmentScopeType ScopeType,
    Optional<string> ScopeIdentifier,
    Optional<int> MaxSuggestions
);