namespace ASP.Web.Core.Templating
{
    public class BreadcrumbViewModel
    {
        public List<BreadcrumbItem> Breadcrumbs { get; set; }
        public string? CurrentPageTitle { get; set; }

        public BreadcrumbViewModel(string? currentPage)
        {
            Breadcrumbs = new List<BreadcrumbItem>();
            CurrentPageTitle = currentPage;
        }

        public BreadcrumbViewModel AddBreadcrumb(string title, string url)
        {
            Breadcrumbs.Add(new BreadcrumbItem(title, url));
            return this;
        }
    }

    public class BreadcrumbItem
    {
        public string Title { get; set; }
        public string Url { get; set; }

        /// <summary>
        /// Gets the TitleId derived from the title.
        /// This is to be used as a part of the HTML element ID.
        /// </summary>
        public string TitleId => Title.ToLower().Replace(" ", "-");

        public BreadcrumbItem(string title, string url)
        {
            Title = title;
            Url = url;
        }
    }
}
