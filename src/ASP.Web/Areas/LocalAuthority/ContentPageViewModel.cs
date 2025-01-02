using ASP.Web.Core.Templating;
using ASP.Web.Shared;

namespace ASP.Web.Areas.LocalAuthority;

public class ContentPageViewModel
{
    public PageViewModel Page { get; }
    public ContentTemplateViewModel ContentTemplate { get; }

    public ContentPageViewModel(
        PageViewModel page, 
        ContentTemplateViewModel contentTemplate
    )
    {
        Page = page;
        ContentTemplate = contentTemplate;
    }
}