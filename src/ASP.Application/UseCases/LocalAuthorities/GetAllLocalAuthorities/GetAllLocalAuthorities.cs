using ASP.Application.UseCases.LocalAuthorities.DTO;
using ASP.Application.UseCases.LocalAuthorities.DTO.Mapper;
using ASP.Core;
using ASP.Core.LocalAuthorities;
using ASP.Core.Results;
using ASP.Core.Utilities;

namespace ASP.Application.UseCases.LocalAuthorities.GetAllLocalAuthorities;

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
        var resultsPerPage = request.ResultsPerPage.GetValueOrDefault(Constants.SearchResultPageSize);
        return await _localAuthorityRepository.GetAllLocalAuthorities(page, resultsPerPage)
            .Map(r => r.Map(x => x.MapToLocalAuthorityDTO()));
    }
}