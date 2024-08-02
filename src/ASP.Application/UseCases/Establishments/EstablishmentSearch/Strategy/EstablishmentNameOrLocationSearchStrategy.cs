using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Core.Search.Strategy;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch.Strategy;

public class EstablishmentNameOrLocationSearchStrategy : EstablishmentSearchStrategy
{
    private readonly ISearchService  _searchService;
    
    public EstablishmentNameOrLocationSearchStrategy(ISearchService searchService, string searchTerm,
        int page, int resultsPerPage) : base(searchTerm, page, resultsPerPage)
    {
        _searchService = searchService;
    }
    
    public override async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResultDTO>>> Execute()
    {
        var results = await _searchService.SearchAsync(SearchTerm, Page, ResultsPerPage);

        return results;
    }
}