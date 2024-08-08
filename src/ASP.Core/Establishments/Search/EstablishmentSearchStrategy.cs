using ASP.Core.Results;

namespace ASP.Core.Establishments.Search;

public abstract class EstablishmentSearchStrategy : IEstablishmentSearchStrategy
{
    protected Scope Scope { get; }
    protected string SearchTerm { get; }
    protected int Page { get; }
    protected int ResultsPerPage { get; }

    protected EstablishmentSearchStrategy(Scope scope, string searchTerm, int page, int resultsPerPage)
    {
        Scope = scope;
        SearchTerm = searchTerm;
        Page = page;
        ResultsPerPage = resultsPerPage;
    }

    public abstract Task<Result<SearchResultsPage<EstablishmentListItem>>> Execute();
}