using ASP.Domain.DataDownloads.UseCases.DTO;

namespace ASP.Domain.DataDownloads.UseCases.GetAvailableDownloads
{
    public class GetAvailableDownloadsResponse
    {
        public int? Year { get; set; }
        public List<Download> Downloads { get; set; }
        public List<AcademicYear> AvailableDates { get; set; }

        public GetAvailableDownloadsResponse(List<Download> downloads, int? year, List<AcademicYear> availableDates)
        {
            Year = year;
            Downloads = downloads;
            AvailableDates = availableDates;
        }
    }
}
