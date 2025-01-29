using ASP.Core;
using ASP.Core.DataDownloads;
using ASP.Core.Establishments;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Time;
using ASP.Core.Utilities;
using Microsoft.Extensions.Options;
using System.IO.Compression;

namespace ASP.Application.UseCases.Downloads.GetDownloadPackage
{
    public class GetDownloadPackage : IGetDownloadPackage
    {
        private readonly DataDownloadOptions _downloadStorageOptions;
        private readonly CurrentTimeProvider _currentTimeProvider;
        private readonly IDataDownloadsScopeValidator _scopeValidator;
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly IBlobStorage _blobStorage;

        public GetDownloadPackage(IOptions<DataDownloadOptions> downloadStorageOptions, CurrentTimeProvider currentTimeProvider,
             IDataDownloadsScopeValidator scopeValidator, IEstablishmentRepository establishmentRepository, IBlobStorage blobStorage)
        {
            _downloadStorageOptions = downloadStorageOptions.Value;
            _currentTimeProvider = currentTimeProvider;
            _scopeValidator = scopeValidator;
            _establishmentRepository = establishmentRepository;
            _blobStorage = blobStorage;
        }

        public Task<Result<FileStreamResponse>> HandleRequest(GetDownloadPackageRequest request)
        {
            var result =
                from scopeIdentifier in _scopeValidator.ValidateScopeIdentifier(request.ScopeIdentifier)
                from scope in _scopeValidator.ValidateScope(request.ScopeType, scopeIdentifier, Optional<int>.None)
                              .MapError(error => error is NotFoundError ? Error.Invalid(GetErrorMessage(request.ScopeType, scopeIdentifier)) : error)
                from binaryData in _blobStorage.DownloadAsync(_downloadStorageOptions.DownloadsConfigContainerName, _downloadStorageOptions.DownloadsConfigFileName)
                              .MapError(error => error is NotFoundError ? Error.Unexpected(error.Message, null) : error)
                from config in DownloadConfig.CheckConfigs(binaryData, _downloadStorageOptions.DownloadsConfigFileName)
                              .MapError(error => error is NotFoundError ? Error.Unexpected(error.Message, null) : error)
                from downloadIds in request.DownloadIds.Select(downloadId => DownloadId.Parse(downloadId))
                              .Combine()
                from validDownloadIds in downloadIds.Select(downloadId => CheckAccess(scope, scopeIdentifier, downloadId))
                              .Combine()
                from fileLocations in validDownloadIds.Select(validDownloadId => CreateFileLocation(config, validDownloadId, request.FileType, _downloadStorageOptions))
                              .Combine()
                from downloads in CreateDownloadPackage(fileLocations)
                select downloads;

            return result;
        }

        private static string GetErrorMessage(DataDownloadScopeType scopeType, string scopeIdentifier)
        {
            return scopeType switch
            {
                DataDownloadScopeType.LA => $"Local Authority with Code \"{scopeIdentifier}\" does not exist.",
                _ => $"Establishment with URN \"{scopeIdentifier}\" does not exist."
            };
        }

        private async Task<Result<DownloadId>> CheckAccess(DataDownloadScope scope, string scopeIdentifier, DownloadId downloadId)
        {
            return await Result.Success(downloadId)
                .Then(async id =>
                {
                    if (scope.ScopeType == DataDownloadScopeType.LA && id.Identifier.Length == 6)
                    {
                        return await _establishmentRepository.GetEstablishmentDetails(id.Identifier)
                            .MapError(error => error is NotFoundError
                                ? Error.NotAllowed($"Identifier \"{id.Identifier}\" is not accessible within the given scope.")
                                : error)
                            .ErrorIf(
                                establishment => establishment.LocalAuthority?.Code != scopeIdentifier,
                                Error.NotAllowed($"Identifier \"{id.Identifier}\" is not accessible within the given scope.")
                            )
                            .Map(_ => id);
                    }

                    return Result.Success(id);
                })
                .ErrorIf(
                    _ => scopeIdentifier != downloadId.Identifier &&
                         !(scope.ScopeType == DataDownloadScopeType.LA && downloadId.Identifier.Length == 6),
                    Error.NotAllowed($"Identifier \"{downloadId.Identifier}\" is not accessible within the given scope.")
                );
        }

        private static Result<FileLocation> CreateFileLocation(List<DownloadConfig> downloadConfig,
                                                   DownloadId downloadId,
                                                   FileType fileType,
                                                   DataDownloadOptions dataDownloadsOptions)
        {
            var config = downloadConfig.FirstOrDefault(id => id.Id == downloadId.ConfigId);

            if (config is null)
            {
                return Error.NotFound($"There is no download config with id \"{downloadId.ConfigId}\".");
            }

            var filePath = config.FilePathPattern.Replace("{urn}", downloadId.Identifier)
                                                 .Replace("{code}", downloadId.Identifier)
                                                 .Replace("{year}", downloadId.Year)
                                                 .Replace("{version}", downloadId.Version.Replace("-", "_"))
                                                 .Replace("{filetype}", fileType.ToString().ToLower());

            var dataDownloadsContainer = dataDownloadsOptions.SourceContainerNames[config.Source];

            return Result.Success(new FileLocation(dataDownloadsContainer, filePath));
        }

        private async Task<Result<FileStreamResponse>> CreateDownloadPackage(IEnumerable<FileLocation> fileLocations)
        {
            var zipStream = new MemoryStream();
            Error? error = null;

            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                foreach (var fileLocation in fileLocations)
                {
                    var entry = archive.CreateEntry(fileLocation.FilePath, CompressionLevel.Fastest);

                    using var entryStream = entry.Open();
                    await _blobStorage.DownloadToAsync(entryStream, fileLocation.Container, fileLocation.FilePath)
                                      .OnError(e => error = e);

                    if (error != null)
                    {
                        return error;
                    }
                }
            }
            zipStream.Position = 0;

            return new FileStreamResponse($"{_currentTimeProvider.CurrentTime:yyyyMMdd_HHmmss}_download.zip", zipStream, "application/zip");
        }
    }
}