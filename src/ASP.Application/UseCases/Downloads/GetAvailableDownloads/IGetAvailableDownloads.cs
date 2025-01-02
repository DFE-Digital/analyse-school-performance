using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.Downloads.GetAvailableDownloads
{
    public interface IGetAvailableDownloads : IUseCase<GetAvailableDownloadsRequest, Result<GetAvailableDownloadsResponse>>
    {
    }
}
