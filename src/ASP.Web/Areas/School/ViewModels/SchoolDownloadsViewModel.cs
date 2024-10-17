namespace ASP.Web.Areas.School.ViewModels;

public class SchoolDownloadsViewModel
{
    public SchoolPageViewModel SchoolPage { get; set; }
    public AvailableDownloadsViewModel AvailableDownloads { get; set; }

    public SchoolDownloadsViewModel(
        SchoolPageViewModel schoolPage,
        AvailableDownloadsViewModel availableDownloads
    )
    {
        SchoolPage = schoolPage;
        AvailableDownloads = availableDownloads;
    }
}