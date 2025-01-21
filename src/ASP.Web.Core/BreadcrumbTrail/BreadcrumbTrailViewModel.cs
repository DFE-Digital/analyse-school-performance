namespace ASP.Web.Core.BreadcrumbTrail
{
    public class BreadcrumbTrailViewModel
    {
        public List<BreadcrumbItem> Breadcrumbs { get; }

        public BreadcrumbTrailViewModel()
        {
            Breadcrumbs = new List<BreadcrumbItem>();
        }

        public BreadcrumbTrailViewModel(IEnumerable<BreadcrumbItem> breadcrumbs)
        {
            Breadcrumbs = breadcrumbs.ToList();
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
