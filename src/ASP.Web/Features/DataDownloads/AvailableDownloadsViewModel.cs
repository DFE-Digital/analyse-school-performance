using ASP.Api.Client.DataDownloads;

namespace ASP.Web.Features.DataDownloads
{
    public class AvailableDownloadsViewModel
    {
        public List<Download> Downloads { get; set; } = new();
        public List<AcademicYear> AvailableDates { get; set; } = new();

        public static AvailableDownloadsViewModel FromAvailableDownloads(GetAvailableDownloadsResponse response)
        {
            return new AvailableDownloadsViewModel
            {
                Downloads = response.Downloads,
                AvailableDates = response.AvailableDates,
            };
        }
    }
}