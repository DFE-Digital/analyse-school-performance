using ASP.Core.Pagination;
using ASP.Core.Results;

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

    public Task<Result<ScopedResultsPage<EstablishmentListing>>> HandleRequest(
        GetAllEstablishmentsRequest request)
    {
        var page = request.Page.GetValueOrDefault(1);
        var resultsPerPage = request.ResultsPerPage.GetValueOrDefault(Core.Constants.SearchResultPageSize);

        return
            from scope in _scopeValidator.ValidateScope(request.Scope)
            from results in _repository.GetAllEstablishments(scope, page, resultsPerPage)
            select results;
    }
}