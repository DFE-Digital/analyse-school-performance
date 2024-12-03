namespace ASP.Web.Areas.Shared.DownloadData.SelectFormat;

public class DownloadDataSelectFormatModel
{
    public string SchoolOrLaLevel { get; }
    public List<(string Text, string? Link)> DownloadLinks { get; }
    public string DownloadOtherDatesUrl { get; }

    public DownloadDataSelectFormatModel(
        string schoolOrLaLevel,
        List<(string Text, string? Link)> downloadLinks,
        string downloadOtherDatesUrl
    )
    {
        SchoolOrLaLevel = schoolOrLaLevel;
        DownloadLinks = downloadLinks;
        DownloadOtherDatesUrl = downloadOtherDatesUrl;
    }
}