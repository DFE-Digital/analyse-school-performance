using ASP.Web.Core.Templating;

namespace ASP.Web.Areas.School;

public class SchoolContentPageViewModel
{
    public SchoolPageViewModel SchoolPage { get; }
    public ContentTemplateViewModel ContentTemplate { get; }

    public SchoolContentPageViewModel(
        SchoolPageViewModel schoolPage,
        ContentTemplateViewModel contentTemplate
    )
    {
        SchoolPage = schoolPage;
        ContentTemplate = contentTemplate;
    }
}