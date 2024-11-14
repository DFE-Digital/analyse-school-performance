using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.School;

public class SchoolsPageParameters
{
    public string Title { get; }
    public string SubTitle { get; }
    public string PaginationUrl { get; }
    public BreadcrumbTrailViewModel BreadcrumbTrailViewModel { get; }
    public string Controller { get; }
    public string ControllerAction { get; }
    public string SearchSuggestionUrl { get; }

    public SchoolsPageParameters(
        string title,
        string subTitle,
        string paginationUrl,
        BreadcrumbTrailViewModel breadcrumbTrailViewModel,
        string controller,
        string controllerAction,
        string searchSuggestionUrl)
    {
        Title = title;
        SubTitle = subTitle;
        PaginationUrl = paginationUrl;
        BreadcrumbTrailViewModel = breadcrumbTrailViewModel;
        Controller = controller;
        ControllerAction = controllerAction;
        SearchSuggestionUrl = searchSuggestionUrl;
    }
}