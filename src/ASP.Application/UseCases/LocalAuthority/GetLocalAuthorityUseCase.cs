using ASP.Core.DTO.LocalAuthority;
using ASP.Core.LocalAuthority;
using ASP.Core.Mapper.LocalAuthority;
using ASP.Core.Results;

namespace ASP.Application.UseCases.LocalAuthority;

public class GetLocalAuthorityUseCase : IGetLocalAuthorityUseCase
{
    private readonly ILocalAuthorityRepository _repository;

    public GetLocalAuthorityUseCase(ILocalAuthorityRepository localAuthorityRepository)
    {
        _repository = localAuthorityRepository ??
                      throw new ArgumentNullException(nameof(localAuthorityRepository));
    }

    public async Task<Result<LocalAuthorityDTO>> HandleRequest(GetLocalAuthorityRequest request)
    {
        return await _repository.GetLocalAuthority(request.Code).Map(x =>
            x.MapToLocalAuthorityDTO());
    }
}