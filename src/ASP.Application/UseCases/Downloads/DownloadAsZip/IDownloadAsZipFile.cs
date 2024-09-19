using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.Downloads.DownloadAsZip
{
    public interface IDownloadAsZipFile : IUseCase<DownloadAsZipFileRequest, Result<FileStreamResponse>>
    {

    }
}
