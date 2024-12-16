using System.Text.Json.Serialization;

namespace ASP.Application.UseCases.Downloads
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
    }
}
