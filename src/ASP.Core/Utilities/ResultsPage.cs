namespace ASP.Core.Utilities;

public class ResultsPage<T>
{
    public int Page { get; set; }
    public int ResultsPerPage { get; set; }
    public int TotalResults { get; set; }
    public IEnumerable<T> Results { get; set; } = new List<T>();

    public ResultsPage() { }

    public ResultsPage(int page, int resultsPerPage, int totalResults, IEnumerable<T> results)
    {
        Page = page;
        ResultsPerPage = resultsPerPage;
        TotalResults = totalResults;
        Results = results;
    }
    
    public ResultsPage(int page, int resultsPerPage, int totalResults, ResultsPage<T> resultsPage)
    {
        Page = page;
        ResultsPerPage = resultsPerPage;
        TotalResults = totalResults;
        Results = resultsPage.Results;
    }

    public ResultsPage<TNew> Map<TNew>(Func<T, TNew> mapFunction)
    {
        return new ResultsPage<TNew>(
            Page,
            ResultsPerPage,
            TotalResults,
            Results.Select(mapFunction)
        );
    }
}