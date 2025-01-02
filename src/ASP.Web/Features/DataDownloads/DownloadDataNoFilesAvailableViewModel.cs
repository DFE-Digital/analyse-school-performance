namespace ASP.Web.Features.DataDownloads;

public class DownloadDataNoDownloadsAvailableViewModel : DownloadDataViewModel
{
    public string SchoolOrLaLevel { get; }
    public int? Year { get; }
    public string? DownloadOtherDatesUrl { get; }

    public DownloadDataNoDownloadsAvailableViewModel(
        string schoolOrLaLevel,
        int? year = null,
        string? downloadOtherDatesUrl = null
    )
    {
        SchoolOrLaLevel = schoolOrLaLevel;
        Year = year;
        DownloadOtherDatesUrl = downloadOtherDatesUrl;
    }
}