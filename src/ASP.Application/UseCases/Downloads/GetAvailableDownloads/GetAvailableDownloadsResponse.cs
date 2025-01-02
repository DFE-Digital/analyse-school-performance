using ASP.Application.UseCases.Downloads.DTO;

namespace ASP.Application.UseCases.Downloads.GetAvailableDownloads
{
    public class GetAvailableDownloadsResponse
    {
        public int? Year { get; set; }
        public List<DownloadDto> Downloads { get; set; }

        public GetAvailableDownloadsResponse(List<DownloadDto> downloads, int? year)
        {
            Year = year;
            Downloads = downloads;
        }
    }
}
