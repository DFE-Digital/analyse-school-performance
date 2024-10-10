using ASP.Core.Results;
using ASP.Core.Utilities;

namespace ASP.Core.LocalAuthorities;

public interface ILocalAuthorityRepository
{
    Task<Result<LocalAuthority>> GetLocalAuthority(string code);

    Task<Result<ResultsPage<LocalAuthority>>> GetAllLocalAuthorities(int page,
        int resultsPerPage, CancellationToken cancellationToken = default);
}