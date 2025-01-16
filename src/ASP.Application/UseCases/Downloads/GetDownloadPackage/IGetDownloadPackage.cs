using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.Downloads.GetDownloadPackage
{
    public interface IGetDownloadPackage : IUseCase<GetDownloadPackageRequest, Result<FileStreamResponse>>
    {

    }
}
