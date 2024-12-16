using ASP.Application.UseCases.Downloads.DTO;
using ASP.Core;
using ASP.Core.LocalAuthorities;
using ASP.Core.Results;
using ASP.Core.Utilities;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ASP.Application.UseCases.Downloads.GetAvailableLADownloads
{
    public class GetAvailableLADownloads : IGetAvailableLADownloads
    {
        private readonly IBlobStorage _blobStorage;
        private readonly ILocalAuthorityRepository _localAuthorityRepository;
        private readonly DataDownloadsOptions _options;

        public GetAvailableLADownloads(IBlobStorage blobStorage, ILocalAuthorityRepository localAuthorityRepository, IOptions<DataDownloadsOptions> options)
        {
            _blobStorage = blobStorage;
            _localAuthorityRepository = localAuthorityRepository;
            _options = options.Value;
        }

        public Task<Result<GetAvailableLADownloadsResponse>> HandleRequest(GetAvailableLADownloadsRequest request)
        {
            var result =
                from _ in _localAuthorityRepository.GetLocalAuthority(request.Code)
                from downloadConfigs in GetDownloadConfigs()
                from downloads in downloadConfigs
                    // Group by source to process all ASP downloads first, then KTS etc.
                    .GroupBy(config => config.Source)
                    // Find list of all matching downloads for each group
                    .Select(configGroup => FindMatchingDownloads(configGroup.Key, configGroup, request))
                    // Turns an IEnumerable<Task<Result<List<DownloadDto>>>> into a Task<Result<IEnumerable<List<DownloadDto>>>>
                    .Combine()
                    // Flattens the IEnumerable<List<DownloadDto>> into a single List<DownloadDto>
                    .Map(d => d.SelectMany(v => v).ToList())
                select new GetAvailableLADownloadsResponse(request.Code, downloads, request.Year.ToNullable());

            return result;
        }

        private Task<Result<List<DownloadConfig>>> GetDownloadConfigs()
        {
            return 
                from binaryData in _blobStorage.DownloadAsync(
                    _options.DownloadsConfigContainerName, 
                    _options.DownloadsConfigFileName
                ).MapError(error => error is NotFoundError ? Error.Unexpected(error.Message) : error)
                from configs in CheckConfigs(binaryData)
                select configs
                    .Where(config => config.Scope is "LocalAuthority")
                    .ToList();
        }

        private Task<Result<List<DownloadDto>>> FindMatchingDownloads(string source, IEnumerable<DownloadConfig> downloadConfigs, GetAvailableLADownloadsRequest request)
        {
            return
                from downloadFiles in GetDownloadFiles(source, request)
                from downloadDtos in MatchFilesWithConfigs(downloadFiles, downloadConfigs, request)
                from filteredDownloads in FilterAndOrderDownloads(downloadDtos, request)
                select filteredDownloads;
        }

        private Result<List<DownloadConfig>> CheckConfigs(BinaryData binaryData)
        {
            try
            {
                List<DownloadConfig> data = binaryData.ToObjectFromJson<List<DownloadConfig>>();

                if (!data.Any())
                {
                    return Error.Unexpected($"The configuration file '{_options.DownloadsConfigFileName}' was empty.");
                }

                return data;
            }
            catch (JsonException)
            {
                return Error.Unexpected($"The configuration file '{_options.DownloadsConfigFileName}' contained invalid JSON.");
            }
        }

        private async Task<Result<List<string>>> GetDownloadFiles(string source, GetAvailableLADownloadsRequest request)
        {
            var path = $"LA/{request.Code}";
            request.Year.IfSome(year => path += $"/{year}");

            if(!_options.SourceContainerNames.ContainsKey(source))
            {
                return Error.Unexpected($"The downloads source '{source}' was not recognised.");
            }

            var containerName = _options.SourceContainerNames[source];

            return await _blobStorage.ListAsync(containerName, path)
                .DefaultIf(e => e is NotFoundError, new());
        }

        private Result<List<DownloadDto>> MatchFilesWithConfigs(IEnumerable<string> files, IEnumerable<DownloadConfig> configs, GetAvailableLADownloadsRequest request)
        {
            return files
                .SelectMany(file => configs.Select(config => CreateDownloadIfMatch(file, config, request.Code)))
                .OfType<DownloadDto>()
                .ToList();
        }

        private Result<List<DownloadDto>> FilterAndOrderDownloads(IEnumerable<DownloadDto> downloadFiles, GetAvailableLADownloadsRequest request)
        {
            request.Year.IfSome(year => downloadFiles = downloadFiles.Where(d => d.Year == year));

            var filteredFiles = downloadFiles
                // Group by base ID
                .GroupBy(d => new ReleaseVersion(d.Version).RemoveVersionSuffix(d.Id))
                .Select(g => g
                    // Sort by version priority
                    .OrderByDescending(d => new ReleaseVersion(d.Version).GetPriority())
                    // Select the highest-priority version
                    .First() 
                )
                .OrderBy(d => d.DatasetType)
                .ThenBy(d => d.Label)
                .ToList();

            if (filteredFiles.Count == 0)
            {
                return Error.NotFound(request.Year.Match(
                    year => $@"There are no downloads available for Local Authority ""{request.Code}"" for the year {year}.",
                    () => $@"There are no downloads available for Local Authority ""{request.Code}""."
                ));
            }

            return filteredFiles;
        }

        private DownloadDto? CreateDownloadIfMatch(string filePath, DownloadConfig config, string laCode)
        {
            var pattern = ConvertToRegex(config.FilePathPattern);

            Match match = Regex.Match(filePath, pattern, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return null;
            }

            string year = match.Groups["year"].Value;
            string code = match.Groups["code"].Value;

            string filetype = match.Groups["filetype"].Value;
            if (!Enum.GetNames<FileType>().Any(x => x.Equals(filetype, StringComparison.OrdinalIgnoreCase)))
            {
                return null; // File type is not valid
            }

            ReleaseVersion? version = new ReleaseVersion(match.Groups["version"].Success ? match.Groups["version"].Value : null);
            string id = version.RawValue is not null
                ? $"{config.Id}-{laCode}-{year}-{version.ToDashedFormat()}"
                : $"{config.Id}-{laCode}-{year}";

            return new DownloadDto
            {
                Id = id,
                DatasetType = new DatasetType(config.DataSetType).ToFriendlyName(),
                Source = new Source(config.Source).ToFriendlyName(),
                Label = config.Label,
                Year = int.Parse(year),
                Version = version?.ToFriendlyName()
            };
        }

        private string ConvertToRegex(string filePathPattern)
        {
            // Convert { and } into % so they don't get escaped - 
            // { and } are regex special characters but % isn't
            var encodePlaceholders = filePathPattern
                .Replace("{", "%")
                .Replace("}", "%");

            // Escape any special characters *before* converting the placeholders to patterns
            // (otherwise the patterns themselves will get escaped
            var escaped = Regex.Escape(encodePlaceholders);

            // Convert each placeholder into its own named group
            var convertPlaceholders = escaped
                .Replace("%code%", @"(?<code>\d+)")
                .Replace("%year%", @"(?<year>\d{4})")
                .Replace("%filetype%", @"(?<filetype>[a-zA-Z]+)")
                .Replace("%version%", @"(?<version>[a-zA-Z0-9_]+)");

            // Make sure we match on the full filepath pattern
            return $"^{convertPlaceholders}$";
        }
    }
}