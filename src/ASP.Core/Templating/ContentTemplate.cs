
namespace ASP.Core.Templating
{
    public class ContentTemplate
    {
        public bool IsPublished { get; set; }
        public string PageTitle { get; }
        public dynamic PageContent { get; }
        public List<TemplateComponent> Views { get; }

        public ContentTemplate(bool isPublished, string pageTitle, dynamic pageContent, List<TemplateComponent> views)
        {
            IsPublished = isPublished;
            PageTitle = pageTitle;
            PageContent = pageContent;
            Views = views;
        }
    }
}
