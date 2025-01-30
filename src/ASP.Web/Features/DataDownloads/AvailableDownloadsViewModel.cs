using ASP.Domain.DataDownloads;
using ASP.Domain.DataDownloads.UseCases.DTO;
using ASP.Domain.DataDownloads.UseCases.GetAvailableDownloads;

namespace ASP.Web.Features.DataDownloads
{
    public class AvailableDownloadsViewModel
    {
        public List<DownloadDto> Downloads { get; set; } = new();
        public List<AcademicYear> AvailableDates { get; set; } = new();

        public static AvailableDownloadsViewModel FromAvailableDownloads(GetAvailableDownloadsResponse response)
        {
            return new AvailableDownloadsViewModel
            {
                Downloads = response.Downloads,
                AvailableDates = AcademicYear.ToAcademicYears(response.Downloads.Select(x => x.Year).Distinct()),
            };
        }
    }
}