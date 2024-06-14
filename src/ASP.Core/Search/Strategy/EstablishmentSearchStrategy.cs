using ASP.Core.Results;

namespace ASP.Core.Search.Strategy;

public abstract class EstablishmentSearchStrategy : IEstablishmentSearchStrategy
{
    protected string SearchTerm { get; }
    protected int Page { get; }

    protected EstablishmentSearchStrategy(string searchTerm)
    {
        SearchTerm = searchTerm;
    }
    
    protected EstablishmentSearchStrategy(string searchTerm, int page)
    {
        SearchTerm = searchTerm;
        Page = page;
    }

    public abstract Task<Result<SearchResult<EstablishmentDetailsSearchResultDTO>>> Execute();
}