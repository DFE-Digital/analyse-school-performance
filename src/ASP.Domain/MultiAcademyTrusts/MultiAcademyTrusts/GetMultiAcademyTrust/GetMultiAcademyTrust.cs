using ASP.Core.Results;
using ASP.Domain.MultiAcademyTrusts.UseCases.DTO;
using ASP.Domain.MultiAcademyTrusts.UseCases.DTO.Mapper;

namespace ASP.Domain.MultiAcademyTrusts.UseCases.GetMultiAcademyTrust;

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