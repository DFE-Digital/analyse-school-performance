using ASP.Core.Results;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ASP.Core.DataDownloads
{
    public class DownloadConfig
    {
        [JsonPropertyName("id")]
        public string Id { get; }

        [JsonPropertyName("filePathPattern")]
        public string FilePathPattern { get; }

        [JsonPropertyName("source")]
        public string Source { get; }

        [JsonPropertyName("scope")]
        public string Scope { get; }

        [JsonPropertyName("dataSetType")]
        public string DataSetType { get; }

        [JsonPropertyName("label")]
        public string Label { get; }

        public DownloadConfig(string id, string filePathPattern, string source, string scope, string dataSetType, string label)
        {
            Id = id;
            FilePathPattern = filePathPattern;
            Source = source;
            Scope = scope;
            DataSetType = dataSetType;
            Label = label;
        }

        public static Result<List<DownloadConfig>> CheckConfigs(BinaryData binaryData, string configFileName)
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
