using ASP.Core.LocalAuthorities;
using ASP.Core.Results;
using ASP.Application.UseCases.LocalAuthorities.DTO;
using ASP.Application.UseCases.LocalAuthorities.DTO.Mapper;

namespace ASP.Application.UseCases.LocalAuthorities.GetLocalAuthority;

public class GetLocalAuthority : IGetLocalAuthority
{
    private readonly ILocalAuthorityRepository _repository;

    public GetLocalAuthority(ILocalAuthorityRepository localAuthorityRepository)
    {
        _repository = localAuthorityRepository ??
                      throw new ArgumentNullException(nameof(localAuthorityRepository));
    }

    public Task<Result<LocalAuthorityDTO>> HandleRequest(GetLocalAuthorityRequest request)
    {
        return 
            from la in _repository.GetLocalAuthority(request.Code)
            select la.MapToLocalAuthorityDTO();
    }
}