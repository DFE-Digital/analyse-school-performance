using ASP.Core.Results;

namespace ASP.Core.Search;

public interface ISearchService
{
    /// <summary>
    /// Performs an asynchronous search operation with the specified parameters.
    /// </summary>
    /// <param name="searchTerm"></param>
    /// <param name="page"></param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains a SearchResult object with the list of search results, results count and the total count</returns>
    Task<Result<SearchResult<EstablishmentDetailsSearchResultDTO>>> SearchAsync(string searchTerm, int page);
}