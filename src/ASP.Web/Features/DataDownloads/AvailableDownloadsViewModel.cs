using ASP.Application.UseCases.Downloads;
using ASP.Application.UseCases.Downloads.DTO;
using ASP.Application.UseCases.Downloads.GetAvailableLADownloads;
using ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads;

namespace ASP.Web.Features.DataDownloads
{
    public class AvailableDownloadsViewModel
    {
        public List<DownloadDto> Downloads { get; set; } = new();
        public List<AcademicYear> AvailableDates { get; set; } = new();

        public static AvailableDownloadsViewModel FromAvailableLADownloads(GetAvailableLADownloadsResponse response)
        {
            return new AvailableDownloadsViewModel
            {
                Downloads = response.Downloads,
                AvailableDates = AcademicYear.ToAcademicYears(response.Downloads.Select(x => x.Year).Distinct()),
            };
        }

        public static AvailableDownloadsViewModel FromAvailableSchoolDownloads(GetAvailableSchoolDownloadsResponse response)
        {
            return new AvailableDownloadsViewModel
            {
                Downloads = response.Downloads,
                AvailableDates = AcademicYear.ToAcademicYears(response.Downloads.Select(x => x.Year).Distinct()),
            };
        }
    }
}