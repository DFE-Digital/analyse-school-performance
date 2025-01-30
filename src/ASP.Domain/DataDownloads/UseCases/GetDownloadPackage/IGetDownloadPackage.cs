using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.DataDownloads.UseCases.GetDownloadPackage
{
    public interface IGetDownloadPackage : IUseCase<GetDownloadPackageRequest, Result<FileStreamResponse>>
    {

    }
}
