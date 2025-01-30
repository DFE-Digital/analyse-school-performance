using ASP.Domain.DataDownloads.UseCases.DTO;
using ASP.Core.Results;
using System.Text.RegularExpressions;

namespace ASP.Domain.DataDownloads.UseCases.GetAvailableDownloads
{
    public class GetAvailableDownloads : IGetAvailableDownloads
    {
        private readonly IDataDownloadsFileProvider _fileProvider;
        private readonly IDataDownloadsScopeValidator _scopeValidator;

        public GetAvailableDownloads(IDataDownloadsFileProvider fileProvider,
            IDataDownloadsScopeValidator scopeValidator)
        {
            _fileProvider = fileProvider;
            _scopeValidator = scopeValidator;
        }

        public Task<Result<GetAvailableDownloadsResponse>> HandleRequest(GetAvailableDownloadsRequest request)
        {
            var result =
                from scope in _scopeValidator.ValidateScope(request.ScopeType, request.ScopeIdentifier, request.Year)
                from downloadConfigs in _fileProvider.GetDownloadConfigs()
                from downloads in downloadConfigs
                    .Where(config => config.Scope == scope.ConfigScope)
                    // Group by source to process all ASP downloads first, then KTS etc.
                    .GroupBy(config => config.Source)
                    // Find list of all matching downloads for each group
                    .Select(configGroup => FindMatchingDownloads(scope, configGroup.Key, configGroup, request))
                    // Turns an IEnumerable<Task<Result<List<DownloadDto>>>> into a Task<Result<IEnumerable<List<DownloadDto>>>>
                    .Combine()
                    // Flattens the IEnumerable<List<DownloadDto>> into a single List<DownloadDto>
                    .Map(d => d.SelectMany(v => v).ToList())
                    .ErrorIf(d => !d.Any(), Error.NotFound(request.Year.Match(
                        year => $"There are no downloads available for {scope} for the year {year}.",
                        () => $"There are no downloads available for {scope}."
                    )))
                select new GetAvailableDownloadsResponse(downloads, request.Year.ToNullable());

            return result;
        }

        private Task<Result<List<DownloadDto>>> FindMatchingDownloads(DataDownloadsScope scope, string source, IEnumerable<DownloadConfig> downloadConfigs, GetAvailableDownloadsRequest request)
        {
            return
                from downloadFiles in _fileProvider.GetDownloadFiles(scope, source, request.Year)
                from downloadDtos in MatchFilesWithConfigs(scope, downloadFiles, downloadConfigs, request)
                from filteredDownloads in FilterAndOrderDownloads(downloadDtos, request)
                select filteredDownloads
                    .Select(d => new DownloadDto {
                        DatasetType = d.DatasetType.ToFriendlyName(),
                        Id = d.Id,
                        Label = d.Label,
                        Source = d.Source.ToFriendlyName(),
                        Version = d.Version?.FriendlyName,
                        Year = d.Year
                    })
                    .ToList();
        }

        private Result<List<Download>> MatchFilesWithConfigs(DataDownloadsScope scope, IEnumerable<string> files, IEnumerable<DownloadConfig> configs, GetAvailableDownloadsRequest request)
        {
            // It looks like there may be a bug in the analyser for CA2021 - see https://github.com/dotnet/roslyn-analyzers/issues?q=is%3Aissue%20state%3Aopen%20CA2021
#pragma warning disable CA2021
            return files
                .SelectMany(file => configs.Select(config => CreateDownloadIfMatch(scope, file, config)))
                .OfType<SuccessResult<Download>>()
                .ToList<Result<Download>>()
                .Combine();
#pragma warning restore CA2021
        }

        private Result<List<Download>> FilterAndOrderDownloads(IEnumerable<Download> downloadFiles, GetAvailableDownloadsRequest request)
        {
            request.Year.IfSome(year => downloadFiles = downloadFiles.Where(d => d.Year == year));

            var filteredFiles = downloadFiles
                // Group by base ID
                .GroupBy(d => d.BaseId)
                .Select(g => g
                    // Sort by version priority
                    .OrderByDescending(d => d.Version?.Priority ?? 0)
                    // Select the highest-priority version
                    .First()
                )
                .OrderBy(d => d.DatasetType.ToFriendlyName())
                .ThenBy(d => d.Label)
                .ToList();

            return filteredFiles;
        }

        private Result<Download> CreateDownloadIfMatch(DataDownloadsScope scope, string filePath, DownloadConfig config)
        {
            var pattern = ConvertToRegex(scope, config.FilePathPattern);

            Match match = Regex.Match(filePath, pattern, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return Error.Invalid($@"File path ""{filePath}"" does not match pattern ""{config.FilePathPattern}""");
            }

            string year = match.Groups["year"].Value;
            string code = match.Groups["code"].Value;

            string filetype = match.Groups["filetype"].Value;
            if (!Enum.GetNames<FileType>().Any(x => x.Equals(filetype, StringComparison.OrdinalIgnoreCase)))
            {
                return Error.Invalid($@"File type ""{filetype}"" is not a valid filetype.");
            }

            if (match.Groups["version"].Success)
            {
                return
                    from version in ReleaseVersion.Validate(match.Groups["version"].Value)
                    select new Download {
                        Id = $"{config.Id}-{scope.Identifier}-{year}-{version.ToDashedFormat()}",
                        DatasetType = new DatasetType(config.DataSetType),
                        Source = new Source(config.Source),
                        Label = config.Label,
                        Year = int.Parse(year),
                        Version = version
                    };
            }

            return new Download {
                Id = $"{config.Id}-{scope.Identifier}-{year}",
                DatasetType = new DatasetType(config.DataSetType),
                Source = new Source(config.Source),
                Label = config.Label,
                Year = int.Parse(year),
            };
        }

        private string ConvertToRegex(DataDownloadsScope scope, string filePathPattern)
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
                .Replace($"%{scope.IdentifierPlaceholderName}%", $@"(?<{scope.IdentifierPlaceholderName}>{scope.IdentifierPlaceholderPattern})")
                .Replace("%year%", @"(?<year>\d{4})")
                .Replace("%filetype%", @"(?<filetype>[a-zA-Z]+)")
                .Replace("%version%", @"(?<version>[a-zA-Z0-9_]+)");

            // Make sure we match on the full filepath pattern
            return $"^{convertPlaceholders}$";
        }
    }
}