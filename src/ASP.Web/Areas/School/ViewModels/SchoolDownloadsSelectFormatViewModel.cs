using ASP.Core.Utilities;

namespace ASP.Web.Areas.School.ViewModels;

public class SchoolDownloadsSelectFormatViewModel
{
    public SchoolPageViewModel SchoolPage { get; set; }
    public List<string> SelectedFiles { get; set; }
    public FileType FileType { get; set; }

    public SchoolDownloadsSelectFormatViewModel(
        SchoolPageViewModel schoolPage,
        List<string> selectedFiles
    )
    {
        SchoolPage = schoolPage;
        SelectedFiles = selectedFiles;
    }

}