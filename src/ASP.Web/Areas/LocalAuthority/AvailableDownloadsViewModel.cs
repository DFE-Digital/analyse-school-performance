using ASP.Application.UseCases.Downloads;
using ASP.Application.UseCases.Downloads.DTO;
using ASP.Application.UseCases.Downloads.GetAvailableLADownloads;

namespace ASP.Web.Areas.LocalAuthority
{
    public class AvailableDownloadsViewModel
    {
        public string? LaCode { get; set; }
        public List<DownloadDto> Downloads { get; set; } = new();

        public List<AcademicYear> AvailableDates { get; set; } = new();

        public static AvailableDownloadsViewModel FromAvailableDownloads(GetAvailableLADownloadsResponse response)
        {
            return new AvailableDownloadsViewModel
            {
                LaCode = response.LaCode,
                Downloads = response.Downloads,
                AvailableDates = AcademicYear.ToAcademicYears(response.Downloads.Select(x => x.Year).Distinct()),
            };
        }
    }
}