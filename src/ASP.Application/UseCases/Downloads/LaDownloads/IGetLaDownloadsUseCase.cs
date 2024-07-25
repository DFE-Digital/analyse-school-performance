using ASP.Core.DTO.Downloads;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.Downloads.LaDownloads
{
    public interface IGetLaDownloadsUseCase : IUseCase<GetLaDownloadsUseCaseRequest, Result<LaDownloadsDetailsDto>>
    {
    }
}
