using ASP.Core.Results;
using ASP.Domain.Establishments.Search;
using ASP.Domain.Establishments.UseCases.DTO;
using ASP.Domain.Establishments.UseCases.DTO.Mapper;

namespace ASP.Domain.Establishments.UseCases.EstablishmentSearch;

public class EstablishmentSearch : IEstablishmentSearch
{
    private readonly IEstablishmentScopeValidator _scopeValidator;
    private readonly EstablishmentSearchService _searchService;

    public EstablishmentSearch(
        IEstablishmentRepository establishmentRepository,
        IEstablishmentScopeValidator scopeValidator
    )
    {
        _scopeValidator = scopeValidator;

        _searchService = new EstablishmentSearchService(establishmentRepository);
    }

    public Task<Result<ScopedSearchResultsPage<EstablishmentListingDTO>>> HandleRequest(
        EstablishmentSearchRequest request)
    {
        var page = request.Page.GetValueOrDefault(1);
        var resultsPerPage = request.ResultsPerPage.GetValueOrDefault(Core.Constants.SearchResultPageSize);
        var scopeIdentifier = request.ScopeIdentifier.GetValueOrDefault("");

        return
            from scope in _scopeValidator.ValidateScope(request.ScopeType, scopeIdentifier)
            from results in _searchService.Search(request.SearchTerm, scope, page, resultsPerPage)
            select results.Map(r => r.MapToEstablishmentListingDTO());
    }
}