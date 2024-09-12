using ASP.Web.Core.Templating;

namespace ASP.Web.Areas.LocalAuthority;

public class LocalAuthorityContentPageViewModel
{
    public LocalAuthorityPageViewModel Page { get; }
    public ContentTemplateViewModel ContentTemplate { get; }

    public LocalAuthorityContentPageViewModel(
        LocalAuthorityPageViewModel page, 
        ContentTemplateViewModel contentTemplate
    )
    {
        Page = page;
        ContentTemplate = contentTemplate;
    }
}