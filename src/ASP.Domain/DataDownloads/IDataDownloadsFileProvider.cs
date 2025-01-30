using ASP.Core.Optionality;
using ASP.Core.Results;

namespace ASP.Domain.DataDownloads
{
    public interface IDataDownloadsFileProvider
    {
        Task<Result<List<string>>> GetDownloadFiles(DataDownloadsScope scope, string source, Optional<int> year);
        Task<Result<List<DownloadConfig>>> GetDownloadConfigs();
        Task<Result<Done>> DownloadFileToAsync(Stream stream, FileLocation location);
    }
}
