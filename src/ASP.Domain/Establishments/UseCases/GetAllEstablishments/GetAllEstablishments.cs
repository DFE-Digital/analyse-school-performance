using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.Establishments.UseCases.DTO;
using ASP.Domain.Establishments.UseCases.DTO.Mapper;

namespace ASP.Domain.Establishments.UseCases.GetAllEstablishments;

public class GetAllEstablishments : IGetAllEstablishments
{
    private readonly IEstablishmentRepository _repository;
    private readonly IEstablishmentScopeValidator _scopeValidator;

    public GetAllEstablishments(
        IEstablishmentRepository establishmentRepository,
        IEstablishmentScopeValidator scopeValidator)
    {
        _repository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
        _scopeValidator = scopeValidator ?? throw new ArgumentNullException(nameof(scopeValidator));
    }

    public Task<Result<ScopedResultsPage<EstablishmentListingDTO>>> HandleRequest(
        GetAllEstablishmentsRequest request)
    {
        var page = request.Page.GetValueOrDefault(1);
        var resultsPerPage = request.ResultsPerPage.GetValueOrDefault(Core.Constants.SearchResultPageSize);
        var scopeIdentifier = request.ScopeIdentifier.GetValueOrDefault("");

        return
            from scope in _scopeValidator.ValidateScope(request.ScopeType, scopeIdentifier)
            from results in _repository.GetAllEstablishments(scope, page, resultsPerPage)
            select results.Map(r => r.MapToEstablishmentListingDTO());
    }
}