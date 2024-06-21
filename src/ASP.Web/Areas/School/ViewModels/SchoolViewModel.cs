using ASP.Web.Core.Templating;

namespace ASP.Web.Areas.School.ViewModels;

public class SchoolViewModel
{
    public EstablishmentDetailsViewModel EstablishmentDetails { get; set; } = default!;
    public ContentTemplateViewModel ContentTemplate { get; set; } = default!;
    public BreadcrumbViewModel? Breadcrumbs { get; set; } = default!;
}