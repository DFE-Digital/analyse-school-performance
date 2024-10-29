using ASP.Core.Results;
using ASP.Core.Utilities;

namespace ASP.Core.LocalAuthorities;

public interface ILocalAuthorityRepository
{
    Task<Result<LocalAuthority>> GetLocalAuthority(string code);

    Task<Result<ResultsPage<LocalAuthority>>> GetAllLocalAuthorities(int page,
        int resultsPerPage, CancellationToken cancellationToken = default);

    Task<Result<List<LocalAuthority>>> LocalAuthoritySearchSuggestionsByLaName(
        string searchTerm, int maxSuggestions, CancellationToken cancellationToken = default);

    Task<Result<List<LocalAuthority>>> LocalAuthoritySearchSuggestionsByLaCode(
        string searchTerm, int maxSuggestions, CancellationToken cancellationToken = default);

    Task<Result<SearchResultsPage<LocalAuthority>>> LocalAuthoritySearchByLaName(
        string searchTerm, int page, int resultsPerPage,
        CancellationToken cancellationToken = default);
}