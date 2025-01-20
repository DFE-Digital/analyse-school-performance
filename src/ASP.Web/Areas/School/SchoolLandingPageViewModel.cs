using ASP.Web.Core.Templating;

namespace ASP.Web.Areas.School;

public class SchoolLandingPageViewModel
{
    public SchoolPageViewModel SchoolPage { get; }
    public EstablishmentDetailsViewModel EstablishmentDetails { get; }
    public ContentTemplateViewModel ContentTemplate { get; }

    public SchoolLandingPageViewModel(
        SchoolPageViewModel schoolPage,
        EstablishmentDetailsViewModel establishmentDetails,
        ContentTemplateViewModel contentTemplate
    )
    {
        SchoolPage = schoolPage;
        EstablishmentDetails = establishmentDetails;
        ContentTemplate = contentTemplate;
    }
}
