using ASP.Core.Pagination;
using ASP.Core.Results;

namespace ASP.Domain.LocalAuthorities;

public interface ILocalAuthorityRepository
{
    Task<Result<LocalAuthority>> Get(LACode code);

    Task<Result<ResultsPage<LocalAuthority>>> GetAll(
        int page,
        int resultsPerPage,
        CancellationToken cancellationToken = default);

    Task<Result<ResultsPage<LocalAuthority>>> Search(
        ILocalAuthoritySearchCriteria criteria,
        int page,
        int resultsPerPage,
        CancellationToken cancellationToken = default);

    Task<Result<List<LocalAuthority>>> SearchSuggestions(
        ILocalAuthoritySearchCriteria criteria,
        int maxSuggestions,
        CancellationToken cancellationToken = default);
}