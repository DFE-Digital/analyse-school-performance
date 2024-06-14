using ASP.Core.Results;

namespace ASP.Core.Search.Strategy;

public interface IEstablishmentSearchStrategy
{
    Task<Result<SearchResult<EstablishmentDetailsSearchResultDTO>>> Execute();
}