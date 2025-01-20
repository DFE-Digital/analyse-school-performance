using ASP.Web.Shared;

namespace ASP.Web.Areas.School;

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