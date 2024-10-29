using ASP.Core.Utilities;

namespace ASP.Core.Establishments.Search;

public class ScopedSearchResultsPage<T> : SearchResultsPage<T>
{
    public string Scope { get; set; } = "";
    public string ScopeIdentifier { get; set; } = "";

    public ScopedSearchResultsPage() { }
    
    public ScopedSearchResultsPage(string searchTerm, string scope, string scopeIdentifier, int page,
        int resultsPerPage, int totalResults, IEnumerable<T> results)
        : base(searchTerm, page, resultsPerPage, totalResults, results)
    {
        Scope = scope;
        ScopeIdentifier = scopeIdentifier;
    }

    public ScopedSearchResultsPage(string searchTerm, string scope, string scopeIdentifier, ResultsPage<T> resultsPage)
        : base(searchTerm, resultsPage)
    {
        Scope = scope;
        ScopeIdentifier = scopeIdentifier;
    }

    public new ScopedSearchResultsPage<TNew> Map<TNew>(Func<T, TNew> mapFunction)
    {
        return new ScopedSearchResultsPage<TNew>(
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