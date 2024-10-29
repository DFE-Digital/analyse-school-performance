namespace ASP.Core.Utilities;

public class SearchResultsPage<T> : ResultsPage<T>
{
    public string SearchTerm { get; set; } = "";

    public SearchResultsPage() { }

    public SearchResultsPage(string searchTerm, int page, int resultsPerPage, int totalResults, IEnumerable<T> results)
        : base(page, resultsPerPage, totalResults, results)
    {
        SearchTerm = searchTerm;
    }

    public SearchResultsPage(string searchTerm, ResultsPage<T> resultsPage)
    : base(resultsPage.Page, resultsPage.ResultsPerPage, resultsPage.TotalResults, resultsPage.Results)
    {
        SearchTerm = searchTerm;
    }

    public new SearchResultsPage<TNew> Map<TNew>(Func<T, TNew> mapFunction)
    {
        return new SearchResultsPage<TNew>(
            SearchTerm,
            Page,
            ResultsPerPage,
            TotalResults,
            Results.Select(mapFunction)
        );
    }
}
