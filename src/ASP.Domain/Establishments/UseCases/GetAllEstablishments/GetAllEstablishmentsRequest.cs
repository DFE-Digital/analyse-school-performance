using ASP.Core.Optionality;

namespace ASP.Domain.Establishments.UseCases.GetAllEstablishments;

public record GetAllEstablishmentsRequest(
    Optional<EstablishmentScopeInfo> Scope,
    Optional<int> Page,
    Optional<int> ResultsPerPage);
