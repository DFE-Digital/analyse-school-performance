namespace ASP.Domain.DataDownloads
{
    public class DataDownloadsOptions
    {
        public const string SectionName = "DataDownloads";

        public string DownloadsConfigContainerName { get; set; } = "";
        public string DownloadsConfigFileName { get; set; } = "";
        public Dictionary<string, string> SourceContainerNames { get; set; } = new();
    }
}
