using ASP.Application.UseCases.Downloads.DTO;

namespace ASP.Application.UseCases.Downloads.GetAvailableLADownloads
{
    public class GetAvailableLADownloadsResponse
    {
        public string LaCode { get; set; }
        public int? Year { get; set; }
        public List<DownloadDto> Downloads { get; set; }

        public GetAvailableLADownloadsResponse(string laCode, List<DownloadDto> downloads, int? year)
        {
            LaCode = laCode;
            Year = year;
            Downloads = downloads;
        }
    }
}
