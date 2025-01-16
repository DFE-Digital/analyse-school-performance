using ASP.Core.Utilities;

namespace ASP.Web.Features.DataDownloads;

public class DownloadDataSelectFormatViewModel : DownloadDataViewModel
{
    public string SchoolOrLaLevel { get; }
    public List<(string Text, string? Link)> DownloadLinks { get; }
    public string DownloadOtherDatesUrl { get; }
    public string FileFormatList { get; } = GetFileFormatList();


    public DownloadDataSelectFormatViewModel(
        string schoolOrLaLevel,
        List<(string Text, string? Link)> downloadLinks,
        string downloadOtherDatesUrl
    )
    {
        SchoolOrLaLevel = schoolOrLaLevel;
        DownloadLinks = downloadLinks;
        DownloadOtherDatesUrl = downloadOtherDatesUrl;
    }

    private static string GetFileFormatList()
    {
        var fileFormats = Enum.GetValues<FileType>();
        var fileFormatList = string.Join(", ", fileFormats[..^1]) + " or " + fileFormats[^1];
        return fileFormatList;
    }
}