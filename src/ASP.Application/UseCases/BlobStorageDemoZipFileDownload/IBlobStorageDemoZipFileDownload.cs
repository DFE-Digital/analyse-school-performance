using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.BlobStorageDemoZipFileDownload;

public interface IBlobStorageDemoZipFileDownload : IUseCase<BlobStorageDemoZipFileDownloadRequest, Result<FileStreamResponse>>
{
}
