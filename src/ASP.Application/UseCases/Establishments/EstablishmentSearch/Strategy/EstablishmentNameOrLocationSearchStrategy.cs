using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Core.Search.Strategy;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch.Strategy;

public class EstablishmentNameOrLocationSearchStrategy : EstablishmentSearchStrategy
{
    private readonly ISearchService  _searchService;
    
    public EstablishmentNameOrLocationSearchStrategy(string searchTerm, int page, ISearchService searchService) : base(searchTerm, page)
    {
        _searchService = searchService;
    }
    
    public override async Task<Result<SearchResult<EstablishmentDetailsSearchResultDTO>>> Execute()
    {
        var results = await _searchService.SearchAsync(SearchTerm, Page);

        return results;
    }
}