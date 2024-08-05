using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.Downloads.GetAvailableLADownloads
{
    public interface IGetAvailableLADownloads : IUseCase<GetAvailableLADownloadsRequest, Result<GetAvailableLADownloadsResponse>>
    {
    }
}
