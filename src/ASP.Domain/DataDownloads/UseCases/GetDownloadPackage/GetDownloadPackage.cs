using ASP.Domain.Establishments;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Time;
using Microsoft.Extensions.Options;
using System.IO.Compression;

namespace ASP.Domain.DataDownloads.UseCases.GetDownloadPackage
{
    public class GetDownloadPackage : IGetDownloadPackage
    {
        private readonly DataDownloadsOptions _downloadStorageOptions;
        private readonly CurrentTimeProvider _currentTimeProvider;
        private readonly IDataDownloadsScopeValidator _scopeValidator;
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly IDataDownloadsFileProvider _fileProvider;

        public GetDownloadPackage(
			IOptions<DataDownloadsOptions> downloadStorageOptions, 
			CurrentTimeProvider currentTimeProvider,
			IDataDownloadsScopeValidator scopeValidator, 
			IEstablishmentRepository establishmentRepository,
			IDataDownloadsFileProvider fileProvider)
        {
            _downloadStorageOptions = downloadStorageOptions.Value;
            _currentTimeProvider = currentTimeProvider;
            _scopeValidator = scopeValidator;
            _establishmentRepository = establishmentRepository;
            _fileProvider = fileProvider;
        }

        public Task<Result<FileStreamResponse>> HandleRequest(GetDownloadPackageRequest request)
        {
            var result =
                from scopeIdentifier in _scopeValidator.ValidateScopeIdentifier(request.ScopeIdentifier)
                from scope in _scopeValidator.ValidateScope(request.ScopeType, scopeIdentifier, Optional<int>.None)
                    .MapError(error => error is NotFoundError ? Error.Invalid(GetErrorMessage(request.ScopeType, scopeIdentifier)) : error)
                from configs in _fileProvider.GetDownloadConfigs()
                from downloadIds in request.DownloadIds
                    .Select(DownloadId.Parse)
                    .Combine()
				from validDownloadIds in downloadIds
					.Select(downloadId => CheckAccess(scope, downloadId))
                    .Combine()
                from fileLocations in validDownloadIds
					.Select(validDownloadId => CreateFileLocation(configs, validDownloadId, request.FileType, _downloadStorageOptions))
                    .Combine()
                from downloads in CreateDownloadPackage(fileLocations)
                select downloads;

            return result;
        }

        private static string GetErrorMessage(DataDownloadsScopeType scopeType, string scopeIdentifier)
        {
            return scopeType switch
            {
                DataDownloadsScopeType.LA => $"Local Authority with Code \"{scopeIdentifier}\" does not exist.",
                _ => $"Establishment with URN \"{scopeIdentifier}\" does not exist."
            };
        }

        private async Task<Result<DownloadId>> CheckAccess(DataDownloadsScope scope, DownloadId downloadId)
        {
            return await Result.Success(downloadId)
                .Then(async id =>
                {
                    if (scope.ScopeType == DataDownloadsScopeType.LA && id.Identifier.Length == 6)
                    {
                        return await _establishmentRepository.GetEstablishmentDetails(id.Identifier)
                            .MapError(error => error is NotFoundError
                                ? Error.NotAllowed($"Identifier \"{id.Identifier}\" is not accessible within the given scope.")
                                : error)
                            .ErrorIf(
                                establishment => establishment.LocalAuthority?.Code != scope.Identifier,
                                Error.NotAllowed($"Identifier \"{id.Identifier}\" is not accessible within the given scope.")
                            )
                            .Map(_ => id);
                    }

                    return Result.Success(id);
                })
                .ErrorIf(
                    _ => scope.Identifier != downloadId.Identifier &&
                         !(scope.ScopeType == DataDownloadsScopeType.LA && downloadId.Identifier.Length == 6),
                    Error.NotAllowed($"Identifier \"{downloadId.Identifier}\" is not accessible within the given scope.")
                );
        }

        private static Result<FileLocation> CreateFileLocation(
            List<DownloadConfig> downloadConfig,
            DownloadId downloadId,
            FileType fileType,
            DataDownloadsOptions dataDownloadsOptions)
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
                    await _fileProvider.DownloadFileToAsync(entryStream, fileLocation)
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