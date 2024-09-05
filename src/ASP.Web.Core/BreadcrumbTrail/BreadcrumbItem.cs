namespace ASP.Web.Core.BreadcrumbTrail
{
    public class BreadcrumbItem
    {
        public string Title { get; }
        public string Url { get; }

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
