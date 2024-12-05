namespace ASP.Web.Areas.Shared.Search;

public abstract class SearchFormParameters : SearchBaseModel
{
    public string SearchUrl { get; }

    protected SearchFormParameters(
        string searchUrl,
        string controller,
        string controllerAction,
        string searchSuggestionUrl
    ) : base(
        controller, 
        controllerAction, 
        searchSuggestionUrl
    )
    {
        SearchUrl = searchUrl;
    }
}