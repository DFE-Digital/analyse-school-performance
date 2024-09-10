using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.DTO.Mapper;
using ASP.Core.Establishments.Search;
using ASP.Core.Results;
using ASP.Core.Establishments;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch;

public class EstablishmentSearch : IEstablishmentSearch
{
    private readonly EstablishmentSearchService _searchService;

    public EstablishmentSearch(
        IEstablishmentRepository establishmentRepository,
        ILocalAuthorityRepository localAuthorityRepository,
        IMultiAcademyTrustRepository multiAcademyTrustRepository
    )
    {
        _searchService = new EstablishmentSearchService(
            establishmentRepository,
            localAuthorityRepository,
            multiAcademyTrustRepository
        );
    }

    public async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResultDTO>>> HandleRequest(
        EstablishmentSearchRequest request)
    {
        var scope = new Scope(request.ScopeType, request.ScopeIdentifier);
        var page = request.Page ?? 1;
        var resultsPerPage = request.ResultsPerPage ?? 50;

        return await _searchService.Search(request.SearchTerm, scope, page, resultsPerPage)
            .Map(results => results.Map(r => r.MapToSearchResultDTO()));
    }
}