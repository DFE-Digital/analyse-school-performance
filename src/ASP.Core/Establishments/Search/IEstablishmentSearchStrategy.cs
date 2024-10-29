using ASP.Core.Results;

namespace ASP.Core.Establishments.Search;

public interface IEstablishmentSearchStrategy
{
    Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> Execute();
}