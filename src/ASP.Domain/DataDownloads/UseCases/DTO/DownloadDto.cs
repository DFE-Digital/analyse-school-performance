namespace ASP.Domain.DataDownloads.UseCases.DTO
{
    public class DownloadDto
    {
        public required string Id { get; set; }
        public required string Label { get; set; }
        public required string Source { get; set; }
        public required int Year { get; set; }
        public required string DatasetType { get; set; }
        public string? Version { get; set; }
    }
}
