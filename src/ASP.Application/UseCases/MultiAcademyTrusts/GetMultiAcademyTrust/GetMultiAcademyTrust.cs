using ASP.Application.UseCases.MultiAcademyTrusts.DTO;
using ASP.Application.UseCases.MultiAcademyTrusts.DTO.Mapper;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Results;

namespace ASP.Application.UseCases.MultiAcademyTrusts.GetMultiAcademyTrust;

public class GetMultiAcademyTrust : IGetMultiAcademyTrust
{
    private readonly IMultiAcademyTrustRepository _repository;

    public GetMultiAcademyTrust(IMultiAcademyTrustRepository multiAcademyTrust)
    {
        _repository = multiAcademyTrust ??
                      throw new ArgumentNullException(nameof(multiAcademyTrust));
    }

    public Task<Result<MultiAcademyTrustDTO>> HandleRequest(GetMultiAcademyTrustRequest request)
    {
        return 
            from mat in _repository.GetMultiAcademyTrust(request.Id)
            select mat.MapToMultiAcademyTrustDTO();
    }
}