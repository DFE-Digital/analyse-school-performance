using ASP.Core.Results;

namespace ASP.Core.Establishments.Search;

public class EstablishmentNameOrLocationSearchStrategy : EstablishmentSearchStrategy
{
    private readonly ISearchService _searchService;

    public EstablishmentNameOrLocationSearchStrategy(ISearchService searchService, Scope scope, string searchTerm,
        int page, int resultsPerPage) : base(scope, searchTerm, page, resultsPerPage)
    {
        _searchService = searchService;
    }

    public override async Task<Result<SearchResultsPage<EstablishmentListItem>>> Execute()
    {
        var results = await _searchService.SearchAsync(Scope, SearchTerm, Page, ResultsPerPage);

        return results;
    }
}