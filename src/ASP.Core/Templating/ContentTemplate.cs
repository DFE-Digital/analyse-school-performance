namespace ASP.Core.Templating
{
    public class ContentTemplate
    {
        public bool IsPublished { get; set; }
        public string PageTitle { get; set; }
        public dynamic PageContent { get; set; }
        public List<TemplateComponent> Views { get; set; }

        public ContentTemplate(bool isPublished, string pageTitle, dynamic pageContent, List<TemplateComponent> views)
        {
            IsPublished = isPublished;
            PageTitle = pageTitle;
            PageContent = pageContent;
            Views = views;
        }
    }
}
