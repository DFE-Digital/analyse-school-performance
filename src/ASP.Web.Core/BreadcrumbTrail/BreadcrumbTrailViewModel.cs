namespace ASP.Web.Core.BreadcrumbTrail
{
    public class BreadcrumbTrailViewModel
    {
        public List<BreadcrumbItem> Breadcrumbs { get; }
        public string CurrentPageTitle { get; }

        public BreadcrumbTrailViewModel(string currentPage)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(currentPage, nameof(currentPage));

            Breadcrumbs = new List<BreadcrumbItem>();
            CurrentPageTitle = currentPage;
        }

        public BreadcrumbTrailViewModel(IEnumerable<BreadcrumbItem> breadcrumbs, string currentPage)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(currentPage, nameof(currentPage));

            Breadcrumbs = breadcrumbs.ToList();
            CurrentPageTitle = currentPage;
        }

        public BreadcrumbTrailViewModel AddBreadcrumb(string title, string url)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title, nameof(title));
            ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

            Breadcrumbs.Add(new BreadcrumbItem(title, url));
            return this;
        }

        public BreadcrumbTrailViewModel Prepend(IEnumerable<BreadcrumbItem> breadcrumbs)
        {
            Breadcrumbs.InsertRange(0, breadcrumbs);
            return this;
        }
    }
}
