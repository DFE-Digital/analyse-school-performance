using ASP.Core.Results;

namespace ASP.Core.DataDownloads
{
    public class DownloadId
    {
        public string ConfigId { get; }
        public string Identifier { get; }
        public string Year { get; }
        public string Version { get; }

        public DownloadId(string configId, string identifier, string year, string version)
        {
            ConfigId = configId;
            Identifier = identifier;
            Year = year;
            Version = version;
        }

        public static Result<DownloadId> Parse(string downloadId)
        {
            var match = Constants.DownloadIdRegex.Match(downloadId);

            if (!match.Success)
            {
                return Error.Invalid($"Download ID: {downloadId} is not in the format \"{{download-config.id}}-{{identifier}}-{{year}}[-{{version}}]\".\"");
            }

            return new DownloadId(
              match.Groups["ConfigId"].Value,
              match.Groups["Identifier"].Value,
              match.Groups["Year"].Value,
              match.Groups["Version"].Value
            );

        }
    }
}
