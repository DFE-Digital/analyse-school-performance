using ASP.Core.Optionality;

namespace ASP.Domain.Establishments.UseCases.EstablishmentSearch;

public record EstablishmentSearchRequest(
    string SearchTerm,
    Optional<EstablishmentScopeInfo> Scope,
    Optional<int> Page,
    Optional<int> ResultsPerPage
);