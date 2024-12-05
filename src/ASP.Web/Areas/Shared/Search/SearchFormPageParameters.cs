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
        string? controller,
        string? controllerAction,
        string searchSuggestionUrl)
        : base(
            searchSuggestionUrl,
            controller,
            controllerAction
        )
    {
        Title = title;
        SubTitle = subTitle;
        PaginationUrl = paginationUrl;
    }
}