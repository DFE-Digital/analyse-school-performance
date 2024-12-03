using ASP.Web.Areas.Shared.DownloadData.SelectFiles;
using ASP.Web.Shared;

namespace ASP.Web.Areas.LocalAuthority;

public class LocalAuthorityDownloadDataSelectFilesViewModel
{
    public PageViewModel Page { get; }
    public DownloadDataSelectFilesModel SelectFiles { get; }

    public LocalAuthorityDownloadDataSelectFilesViewModel(
        PageViewModel page,
        DownloadDataSelectFilesModel selectFiles
    )
    {
        Page = page;
        SelectFiles = selectFiles;
    }
}
