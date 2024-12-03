using ASP.Web.Areas.Shared.DownloadData.SelectFormat;

namespace ASP.Web.Areas.School.ViewModels;

public class SchoolDownloadDataSelectFormatViewModel
{
    public SchoolPageViewModel SchoolPage { get; }
    public DownloadDataSelectFormatModel SelectFormat { get; }

    public SchoolDownloadDataSelectFormatViewModel(
        SchoolPageViewModel schoolPage,
        DownloadDataSelectFormatModel selectFormat
    )
    {
        SchoolPage = schoolPage;
        SelectFormat = selectFormat;
    }
}