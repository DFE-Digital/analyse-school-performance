using ASP.Core.DTO.Downloads;

namespace ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads
{
    public class GetAvailableSchoolDownloadsResponse
    {
        public string Urn { get; set; }
        public List<DownloadsDetailsDto> DownloadItems { get; set; }

        public GetAvailableSchoolDownloadsResponse(string urn, List<DownloadsDetailsDto> downloadItems)
        {
            Urn = urn;
            DownloadItems = downloadItems;
        }
    }
}