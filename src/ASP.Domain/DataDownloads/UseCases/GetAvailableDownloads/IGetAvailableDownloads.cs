using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.DataDownloads.UseCases.GetAvailableDownloads
{
    public interface IGetAvailableDownloads : IUseCase<GetAvailableDownloadsRequest, Result<GetAvailableDownloadsResponse>>
    {
    }
}
