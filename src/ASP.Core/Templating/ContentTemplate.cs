namespace ASP.Core.Templating
{
    public class ContentTemplate
    {
        public string PageTitle { get; }
        public dynamic PageContent { get; }
        public List<TemplateComponent> Views { get; }

        public ContentTemplate(string pageTitle, dynamic pageContent, List<TemplateComponent> views)
        {
            PageTitle = pageTitle;
            PageContent = pageContent;
            Views = views;
        }
    }
}
