using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.LocalAuthorities.UseCases.DTO;
using ASP.Domain.LocalAuthorities.UseCases.DTO.Mapper;

namespace ASP.Domain.LocalAuthorities.UseCases.GetAllLocalAuthorities;

public class GetAllLocalAuthorities : IGetAllLocalAuthorities
{
    private readonly ILocalAuthorityRepository _localAuthorityRepository;

    public GetAllLocalAuthorities(ILocalAuthorityRepository localAuthorityRepository)
    {
        _localAuthorityRepository = localAuthorityRepository ??
                                    throw new ArgumentNullException(nameof(localAuthorityRepository));
    }

    public async Task<Result<ResultsPage<LocalAuthorityDTO>>> HandleRequest(GetAllLocalAuthoritiesRequest request)
    {
        var page = request.Page.GetValueOrDefault(1);
        var resultsPerPage = request.ResultsPerPage.GetValueOrDefault(Core.Constants.SearchResultPageSize);
        return await _localAuthorityRepository.GetAllLocalAuthorities(page, resultsPerPage)
            .Map(r => r.Map(x => x.MapToLocalAuthorityDTO()));
    }
}