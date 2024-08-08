using ASP.Application.UseCases.MultiAcademyTrusts.DTO;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.MultiAcademyTrusts.GetMultiAcademyTrust;

public interface IGetMultiAcademyTrust : IUseCase<GetMultiAcademyTrustRequest, Result<MultiAcademyTrustDTO>>
{
    
}