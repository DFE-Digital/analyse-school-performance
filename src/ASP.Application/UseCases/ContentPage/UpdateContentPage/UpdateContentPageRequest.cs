using ASP.Core.PageContent;

namespace ASP.Application.UseCases.UpdateContentPage
{
    public class UpdateContentPageRequest
    {
        public string PageContentId { get; set; }
        public PageContentTemplate PageContentTemplate { get; set; }

        public UpdateContentPageRequest(string pageContentId, PageContentTemplate pageContentTemplate)
        {
            PageContentId = pageContentId;
            PageContentTemplate = pageContentTemplate;
        }
    }
}
