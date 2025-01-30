using ASP.Domain.DataDownloads;

namespace ASP.Web.Features.DataDownloads;

public class DownloadDataSelectYearViewModel : DownloadDataViewModel
{
    public List<AcademicYear> AvailableDates { get; }

    public DownloadDataSelectYearViewModel(
        List<AcademicYear> availableDates
    )
    {
        AvailableDates = availableDates;
    }
}