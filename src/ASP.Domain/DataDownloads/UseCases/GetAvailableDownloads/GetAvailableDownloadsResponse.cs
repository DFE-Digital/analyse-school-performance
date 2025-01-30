using ASP.Domain.DataDownloads.UseCases.DTO;

namespace ASP.Domain.DataDownloads.UseCases.GetAvailableDownloads
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
