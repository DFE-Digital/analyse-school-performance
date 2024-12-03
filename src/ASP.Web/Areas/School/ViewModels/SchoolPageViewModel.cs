using ASP.Web.Shared;

namespace ASP.Web.Areas.School.ViewModels;

public class SchoolPageViewModel
{
    public string Urn { get; set; }
    public PageViewModel Page { get; set; }

    public SchoolPageViewModel(
        string urn,
        PageViewModel page
    )
    {
        Urn = urn;
        Page = page;
    }
}