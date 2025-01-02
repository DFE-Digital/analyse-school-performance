using ASP.Application.UseCases.Downloads.DTO;
using ASP.Application.UseCases.Downloads.GetAvailableDownloads;
using ASP.Core.DataDownloads;

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