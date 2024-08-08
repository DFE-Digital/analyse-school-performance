using ASP.Core.Utilities;

namespace ASP.Core.Establishments.Search;

public class SearchResultsPage<T> : ResultsPage<T>
{
    public string SearchTerm { get; set; } = "";
    public string Scope { get; set; } = "";
    public string ScopeIdentifier { get; set; } = "";

    public SearchResultsPage() { }

    public SearchResultsPage(string searchTerm, string scope, string scopeIdentifier, int page, int resultsPerPage, int totalResults, IEnumerable<T> results)
        : base(page, resultsPerPage, totalResults, results)
    {
        SearchTerm = searchTerm;
        Scope = scope;
        ScopeIdentifier = scopeIdentifier;
    }

    public SearchResultsPage(string searchTerm, string scope, string scopeIdentifier, ResultsPage<T> resultsPage)
    : base(resultsPage.Page, resultsPage.ResultsPerPage, resultsPage.TotalResults, resultsPage.Results)
    {
        SearchTerm = searchTerm;
        Scope = scope;
        ScopeIdentifier = scopeIdentifier;
    }

    public new SearchResultsPage<TNew> Map<TNew>(Func<T, TNew> mapFunction)
    {
        return new SearchResultsPage<TNew>(
            SearchTerm,
            Scope,
            ScopeIdentifier,
            Page,
            ResultsPerPage,
            TotalResults,
            Results.Select(mapFunction)
        );
    }
}
