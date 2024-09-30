namespace ASP.Application.UseCases.Downloads.DTO
{
    public class DownloadDto
    {
        public string? Id { get; set; }
        public string? Label { get; set; }

        public string? Source { get; set; }
        public int Year { get; set; }
        public string? DatasetType { get; set; }

        public string? Version { get; set; }
    }
}
