using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.BlobStorageDemoFileDownload;

public interface IBlobStorageDemoFileDownload : IUseCase<BlobStorageDemoFileDownloadRequest, Result<FileStreamResponse>>
{

}
