using ASP.Core.Optionality;

namespace ASP.Domain.Establishments.UseCases.EstablishmentSearch;

public record EstablishmentSearchRequest(
    string SearchTerm,
    EstablishmentScopeType ScopeType,
    Optional<string> ScopeIdentifier,
    Optional<int> Page,
    Optional<int> ResultsPerPage
);