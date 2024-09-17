using ASP.Core.Utilities;

namespace ASP.Core.Scoping;

public class ScopedResultsPage<T> : ResultsPage<T>
{
    public string Scope { get; set; } = "";
    public string ScopeIdentifier { get; set; } = "";

    public ScopedResultsPage()
    {
    }

    public ScopedResultsPage(string scope, string scopeIdentifier, int page, int resultsPerPage,
        int totalResults, IEnumerable<T> results)
        : base(page, resultsPerPage, totalResults, results)
    {
        Scope = scope;
        ScopeIdentifier = scopeIdentifier;
    }

    public ScopedResultsPage(string scope, string scopeIdentifier, ResultsPage<T> resultsPage)
        : base(resultsPage.Page, resultsPage.ResultsPerPage, resultsPage.TotalResults, resultsPage.Results)
    {
        Scope = scope;
        ScopeIdentifier = scopeIdentifier;
    }

    public new ScopedResultsPage<TNew> Map<TNew>(Func<T, TNew> mapFunction)
    {
        return new ScopedResultsPage<TNew>(
            Scope,
            ScopeIdentifier,
            Page,
            ResultsPerPage,
            TotalResults,
            Results.Select(mapFunction)
        );
    }
}