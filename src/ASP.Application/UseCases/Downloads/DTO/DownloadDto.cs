namespace ASP.Application.UseCases.Downloads.DTO
{
    public class DownloadDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }

        public string? DownloadSource { get; set; }
        public string? Year { get; set; }
        public string? DatasetType { get; set; }

        public string? ReleaseVersion { get; set; }
    }
}
