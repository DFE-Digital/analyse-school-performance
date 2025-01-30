using ASP.Domain.DataDownloads;

namespace ASP.Web.Features.DataDownloads
{
    public record DownloadDataStepParameters
    {
        public string? Step { get; set; }
        public int? SelectedYear { get; set; }
        public List<string>? SelectedFiles { get; set; }
        public FileType? FileType { get; set; }

        public RouteValueDictionary AsRouteValues()
            => new(new { Step, SelectedYear, SelectedFiles, FileType });
    }
}