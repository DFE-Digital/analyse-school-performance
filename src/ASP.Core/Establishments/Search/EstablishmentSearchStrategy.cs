using ASP.Core.Results;

namespace ASP.Core.Establishments.Search;

public abstract class EstablishmentSearchStrategy : IEstablishmentSearchStrategy
{
    protected string SearchTerm { get; }
    protected int Page { get; }
    protected int ResultsPerPage { get; }

    protected EstablishmentSearchStrategy(string searchTerm, int page, int resultsPerPage)
    {
        SearchTerm = searchTerm;
        Page = page;
        ResultsPerPage = resultsPerPage;
    }

    public abstract Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> Execute();
}