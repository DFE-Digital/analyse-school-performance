using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Core.Templating;

namespace ASP.Web.Areas.School.ViewModels;

public class SchoolViewModel
{
    public string Title { get; }
    public PathString RequestPath { get; }
    public PathString BasePath { get; }
    public EstablishmentDetailsViewModel EstablishmentDetails { get; }
    public ContentTemplateViewModel ContentTemplate { get; }
    public BreadcrumbTrailViewModel? Breadcrumbs { get; }

    public SchoolViewModel(string title, PathString requestPath, PathString basePath, EstablishmentDetailsViewModel establishmentDetails, ContentTemplateViewModel contentTemplate, BreadcrumbTrailViewModel? breadcrumbs)
    {
        Title = title;
        RequestPath = requestPath;
        BasePath = basePath;
        EstablishmentDetails = establishmentDetails;
        ContentTemplate = contentTemplate;
        Breadcrumbs = breadcrumbs;
    }
}