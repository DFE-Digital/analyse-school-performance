using ASP.Api.Client.DataDownloads;
using ASP.Web.Extensions;
using ASP.Web.Features.SubController;

namespace ASP.Web.Features.DataDownloads
{
    public record DownloadDataParameters : SubActionParameters
    {
        public static new readonly List<string> RouteValueKeys = [..SubActionParameters.RouteValueKeys, "selectedYear", "selectedFiles", "fileType"];

        public int? SelectedYear { get; set; }
        public List<string>? SelectedFiles { get; set; }
        public FileType? FileType { get; set; }

        public override RouteValueDictionary AsRouteValues()
            => new(base.AsRouteValues().Merge(new { selectedYear = SelectedYear, selectedFiles = SelectedFiles, fileType = FileType }));
    }
}