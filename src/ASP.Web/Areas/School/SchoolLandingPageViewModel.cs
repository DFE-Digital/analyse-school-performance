using ASP.Web.Core.Templating;

namespace ASP.Web.Areas.School;

public class SchoolLandingPageViewModel
{
    public SchoolPageViewModel SchoolPage { get; }
    public EstablishmentDetailsViewModel EstablishmentDetails { get; }
    public ContentTemplateViewModel ContentTemplate { get; }
    public LinkedEstablishmentsViewModel? LinkedEstablishments { get;  }

    public SchoolLandingPageViewModel(
        SchoolPageViewModel schoolPage,
        EstablishmentDetailsViewModel establishmentDetails,
        ContentTemplateViewModel contentTemplate,
        LinkedEstablishmentsViewModel? linkedEstablishments = null
    )
    {
        SchoolPage = schoolPage;
        EstablishmentDetails = establishmentDetails;
        ContentTemplate = contentTemplate;
        LinkedEstablishments = linkedEstablishments;
    }
}
