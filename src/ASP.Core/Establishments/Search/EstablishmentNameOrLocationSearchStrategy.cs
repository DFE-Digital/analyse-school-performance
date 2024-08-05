using ASP.Core.Results;

namespace ASP.Core.Establishments.Search;

public class EstablishmentNameOrLocationSearchStrategy : EstablishmentSearchStrategy
{
    private readonly ISearchService _searchService;

    public EstablishmentNameOrLocationSearchStrategy(ISearchService searchService, string searchTerm,
        int page, int resultsPerPage) : base(searchTerm, page, resultsPerPage)
    {
        _searchService = searchService;
    }

    public override async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> Execute()
    {
        var results = await _searchService.SearchAsync(SearchTerm, Page, ResultsPerPage);

        return results;
    }
}