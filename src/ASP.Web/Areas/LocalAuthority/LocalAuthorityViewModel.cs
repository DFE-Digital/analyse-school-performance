using ASP.Web.Core.Templating;

namespace ASP.Web.Areas.LocalAuthority;

public class LocalAuthorityViewModel
{
    public string Name { get; set; }
    public ContentTemplateViewModel ContentTemplate { get; set; } = default!;
    public BreadcrumbViewModel? Breadcrumbs { get; set; } = default!;
}