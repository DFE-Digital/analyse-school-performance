using ASP.Application.UseCases.Downloads;

namespace ASP.Web.Areas.Shared.DownloadData.SelectYear;

public class DownloadDataSelectYearModel
{
    public List<AcademicYear> AvailableDates { get; }

    public DownloadDataSelectYearModel(
        List<AcademicYear> availableDates 
    )
    {
        AvailableDates = availableDates;
    }
}