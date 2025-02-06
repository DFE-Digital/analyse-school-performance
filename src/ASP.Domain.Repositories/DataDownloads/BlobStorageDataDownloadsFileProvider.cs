using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Domain.DataDownloads;
using ASP.Infrastructure.Blob;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ASP.Domain.Repositories.DataDownloads
{
    public class BlobStorageDataDownloadsFileProvider : IDataDownloadsFileProvider
    {
        private readonly IBlobStorage _blobStorage;
        private readonly DataDownloadsOptions _options;

        public BlobStorageDataDownloadsFileProvider(IBlobStorage blobStorage, IOptions<DataDownloadsOptions> options)
        {
            _blobStorage = blobStorage;
            _options = options.Value;
        }

        public async Task<Result<List<DownloadConfig>>> GetDownloadConfigs()
        {
            return
                from binaryData in await _blobStorage.DownloadAsync(_options.DownloadsConfigContainerName, _options.DownloadsConfigFileName)
                  .MapError(error => error is NotFoundError ? Error.Unexpected(error.Message, null) : error)
                from config in CheckConfigs(binaryData, _options.DownloadsConfigFileName)
                  .MapError(error => error is NotFoundError ? Error.Unexpected(error.Message, null) : error)
                select config;
        }

        public async Task<Result<List<string>>> GetDownloadFiles(DataDownloadsScope scope, string source, Optional<int> year)
        {
            if (!_options.SourceContainerNames.ContainsKey(source))
            {
                return Error.Unexpected($"The downloads source '{source}' was not recognised.");
            }

            var containerName = _options.SourceContainerNames[source];

            var path = scope.BuildPathPrefix(year);

            return await _blobStorage.ListAsync(containerName, path)
                .DefaultIf(e => e is NotFoundError, new());
        }

        public Task<Result<Done>> DownloadFileToAsync(Stream stream, FileLocation fileLocation)
        {
            return _blobStorage.DownloadToAsync(stream, fileLocation.Container, fileLocation.FilePath);
        }

        private static Result<List<DownloadConfig>> CheckConfigs(BinaryData binaryData, string configFileName)
        {
            try
            {
                List<DownloadConfig> data = binaryData.ToObjectFromJson<List<DownloadConfig>>();

                if (!data.Any())
                {
                    return Error.Unexpected($"The configuration file '{configFileName}' was empty.");
                }

                return data;
            }
            catch (JsonException)
            {
                return Error.Unexpected($"The configuration file '{configFileName}' contained invalid JSON.");
            }
        }
    }
}
