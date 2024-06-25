namespace ASP.Core.Templating
{
    public sealed class TemplateComponent
    {
        public string ViewId { get; set; }
        public dynamic ViewContent { get; set; }
        public dynamic ViewModel { get; set; }
        public List<TemplateComponent> ChildViews { get; set; }

        public TemplateComponent(string viewId, dynamic viewContent, dynamic viewModel, List<TemplateComponent> childViews)
        {
            ViewId = viewId;
            ViewContent = viewContent;
            ViewModel = viewModel;
            ChildViews = childViews;
        }
    }
}
