using ASP.Core.Pagination;
using ASP.Core.Results;

namespace ASP.Domain.LocalAuthorities.UseCases.GetAllLocalAuthorities;

public class GetAllLocalAuthorities : IGetAllLocalAuthorities
{
    private readonly ILocalAuthorityRepository _localAuthorityRepository;

    public GetAllLocalAuthorities(ILocalAuthorityRepository localAuthorityRepository)
    {
        _localAuthorityRepository = localAuthorityRepository ??
                                    throw new ArgumentNullException(nameof(localAuthorityRepository));
    }

    public async Task<Result<ResultsPage<LocalAuthority>>> HandleRequest(GetAllLocalAuthoritiesRequest request)
    {
        var page = request.Page.GetValueOrDefault(1);
        var resultsPerPage = request.ResultsPerPage.GetValueOrDefault(Core.Constants.SearchResultPageSize);
        return await _localAuthorityRepository.GetAll(page, resultsPerPage);
    }
}