using ASP.Core.Optionality;

namespace ASP.Domain.Establishments.UseCases.GetAllEstablishments;

public record GetAllEstablishmentsRequest(
    EstablishmentScopeType ScopeType,
    Optional<string> ScopeIdentifier,
    Optional<int> Page,
    Optional<int> ResultsPerPage);
