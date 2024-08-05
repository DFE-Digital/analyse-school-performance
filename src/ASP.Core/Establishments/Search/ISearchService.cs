using ASP.Core.Results;

namespace ASP.Core.Establishments.Search;

public interface ISearchService
{
    /// <summary>
    /// Performs an asynchronous search operation with the specified parameters.
    /// </summary>
    /// <param name="searchTerm"></param>
    /// <param name="page"></param>
    /// <param name="resultsPerPage"></param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains a SearchResult object with the list of search results, results count and the total count</returns>
    Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> SearchAsync(string searchTerm, int page = 1, int resultsPerPage = 50);
}