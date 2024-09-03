namespace ASP.Core.Templating
{
    public sealed class TemplateComponent
    { 
        public string ViewId { get; }
        public dynamic ViewContent { get; }
        public dynamic ViewModel { get; }
        public List<TemplateComponent> ChildViews { get; }

        public TemplateComponent(string viewId, dynamic viewContent, dynamic viewModel, List<TemplateComponent> childViews)
        {
            ViewId = viewId;
            ViewContent = viewContent;
            ViewModel = viewModel;
            ChildViews = childViews;
        }
    }
}
