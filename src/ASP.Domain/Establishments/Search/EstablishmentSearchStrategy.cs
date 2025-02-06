using ASP.Core.Pagination;
using ASP.Core.Results;

namespace ASP.Domain.Establishments.Search;

public abstract class EstablishmentSearchStrategy : IEstablishmentSearchStrategy
{
    protected EstablishmentScope Scope { get; }
    protected string SearchTerm { get; }
    protected int Page { get; }
    protected int ResultsPerPage { get; }

    protected EstablishmentSearchStrategy(EstablishmentScope scope, string searchTerm, int page, int resultsPerPage)
    {
        Scope = scope;
        SearchTerm = searchTerm;
        Page = page;
        ResultsPerPage = resultsPerPage;
    }

    public abstract Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> Execute();
}