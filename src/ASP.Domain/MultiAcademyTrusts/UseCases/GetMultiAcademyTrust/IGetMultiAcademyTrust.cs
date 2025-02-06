using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.MultiAcademyTrusts.UseCases.GetMultiAcademyTrust;

public interface IGetMultiAcademyTrust : IUseCase<GetMultiAcademyTrustRequest, Result<MultiAcademyTrust>>
{
    
}