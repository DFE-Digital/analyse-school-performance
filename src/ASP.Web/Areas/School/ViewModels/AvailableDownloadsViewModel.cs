using ASP.Application.UseCases.Downloads.DTO;
using ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads;
using ASP.Web.Areas.Shared.Dates;

namespace ASP.Web.Areas.School.ViewModels
{
    public class AvailableDownloadsViewModel
    {
        public string? Urn { get; set; }
        public List<DownloadDto> Downloads { get; set; } = new();

        public List<AcademicYear> AvailableDates { get; set; } = new();

        public static AvailableDownloadsViewModel FromAvailableDownloads(GetAvailableSchoolDownloadsResponse response)
        {
            return new AvailableDownloadsViewModel
            {
                Urn = response.Urn,
                Downloads = response.Downloads,
                AvailableDates = CreateDates(response.Downloads.Select(x => x.Year).Distinct().ToList()),
            };
        }

        private static List<AcademicYear> CreateDates(List<int> years)
        {
            var yearTypes = new List<AcademicYear>();
            foreach (var year in years.Distinct())
            {
                yearTypes.Add(new AcademicYear()
                {
                    Year = year,
                    StartToEndYears = $"{year - 1} to {year}"
                });
            }
            return yearTypes;
        }
    }
}