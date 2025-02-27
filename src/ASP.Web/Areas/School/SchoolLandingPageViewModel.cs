using ASP.Web.Core.Templating;

namespace ASP.Web.Areas.School;

public class SchoolLandingPageViewModel
{
    public SchoolPageViewModel SchoolPage { get; }
    public SchoolDetailsViewModel SchoolDetails { get; }
    public ContentTemplateViewModel ContentTemplate { get; }
    public LinkedSchoolsViewModel? LinkedSchools { get;  }

    public SchoolLandingPageViewModel(
        SchoolPageViewModel schoolPage,
        SchoolDetailsViewModel schoolDetails,
        ContentTemplateViewModel contentTemplate,
        LinkedSchoolsViewModel? linkedSchools = null
    )
    {
        SchoolPage = schoolPage;
        SchoolDetails = schoolDetails;
        ContentTemplate = contentTemplate;
        LinkedSchools = linkedSchools;
    }
}
