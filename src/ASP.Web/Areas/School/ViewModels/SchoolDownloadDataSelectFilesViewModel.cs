using ASP.Web.Areas.Shared.DownloadData.SelectFiles;

namespace ASP.Web.Areas.School.ViewModels;

public class SchoolDownloadDataSelectFilesViewModel
{
    public SchoolPageViewModel SchoolPage { get; }
    public DownloadDataSelectFilesModel SelectFiles { get; }

    public SchoolDownloadDataSelectFilesViewModel(
        SchoolPageViewModel schoolPage,
        DownloadDataSelectFilesModel selectFiles
    )
    {
        SchoolPage = schoolPage;
        SelectFiles = selectFiles;
    }
}