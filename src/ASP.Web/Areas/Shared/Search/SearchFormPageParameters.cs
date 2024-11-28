using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search;

public abstract class SearchFormPageParameters : SearchBaseModel
{
    public string Title { get; }
    public string SubTitle { get; }
    public string PaginationUrl { get; }

    protected SearchFormPageParameters(
        string title,
        string subTitle,
        string paginationUrl,
        string controller,
        string controllerAction,
        string searchSuggestionUrl,
        string inputLabel,
        string inputValidationMessage,
        BreadcrumbTrailViewModel breadcrumbTrail)
        : base(controller, controllerAction, searchSuggestionUrl, inputLabel, inputValidationMessage, breadcrumbTrail)
    {
        Title = title;
        SubTitle = subTitle;
        PaginationUrl = paginationUrl;
    }
}