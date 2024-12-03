using ASP.Web.Areas.Shared.DownloadData.SelectFormat;
using ASP.Web.Shared;

namespace ASP.Web.Areas.LocalAuthority;

public class LocalAuthorityDownloadDataSelectFormatViewModel
{
    public PageViewModel Page { get; }
    public DownloadDataSelectFormatModel SelectFormat { get; }

    public LocalAuthorityDownloadDataSelectFormatViewModel(
        PageViewModel page,
        DownloadDataSelectFormatModel selectFormat
    )
    {
        Page = page;
        SelectFormat = selectFormat;
    }
}