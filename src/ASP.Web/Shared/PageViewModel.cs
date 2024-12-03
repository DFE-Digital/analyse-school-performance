using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Shared.Navigation;

namespace ASP.Web.Shared
{
    public class PageViewModel
    {
        public BreadcrumbTrailViewModel Breadcrumbs { get; }
        public string Title { get; }
        public string? Subtitle { get; }
        public NavigationViewModel? SubNavigation { get; set; }
        public NavigationViewModel? SideNavigation { get; set; }
        public string? ContentTitle { get; }
        public string? ContentTitleCaption { get; }

        public PageViewModel(
            BreadcrumbTrailViewModel breadcrumbs,
            string title,
            string? subtitle = null,
            NavigationViewModel? subNavigation = null,
            NavigationViewModel? sideNavigation = null,
            string? contentTitle = null,
            string? contentTitleCaption = null
        )
        {
            Breadcrumbs = breadcrumbs;
            Title = title;
            Subtitle = subtitle;
            SubNavigation = subNavigation;
            SideNavigation = sideNavigation;
            ContentTitle = contentTitle;
            ContentTitleCaption = contentTitleCaption;
        }
    }
}