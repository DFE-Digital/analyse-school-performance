using ASP.Application.UseCases.Downloads.DTO;

namespace ASP.Application.UseCases.Downloads.GetAvailableLADownloads
{
    public class GetAvailableLADownloadsResponse
    {
        public string Code { get; set; }
        public int? Year { get; set; }
        public List<DownloadDto> Downloads { get; set; }

        public GetAvailableLADownloadsResponse(string code, List<DownloadDto> downloads, int? year)
        {
            Code = code;
            Year = year;
            Downloads = downloads;
        }
    }
}
