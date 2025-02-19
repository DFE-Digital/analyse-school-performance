using ASP.Core.Optionality;

namespace ASP.Domain.Establishments.UseCases.EstablishmentSearchSuggestions;

public record EstablishmentSearchSuggestionsRequest(
    string SearchTerm,
    Optional<EstablishmentScopeInfo> Scope,
    Optional<int> MaxSuggestions
);