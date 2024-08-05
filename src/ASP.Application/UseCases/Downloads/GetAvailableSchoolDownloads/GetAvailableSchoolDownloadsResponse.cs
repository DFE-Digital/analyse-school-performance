using ASP.Application.UseCases.Downloads.DTO;

namespace ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads
{
    public class GetAvailableSchoolDownloadsResponse
    {
        public string Urn { get; set; }
        public List<DownloadDto> Downloads { get; set; }

        public GetAvailableSchoolDownloadsResponse(string urn, List<DownloadDto> downloads)
        {
            Urn = urn;
            Downloads = downloads;
        }
    }
}