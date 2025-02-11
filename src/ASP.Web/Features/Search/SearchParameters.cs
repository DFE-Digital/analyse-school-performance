using ASP.Web.Extensions;
using ASP.Web.Features.SubController;

namespace ASP.Web.Features.Search;

public record SearchParameters : SubActionParameters
{
    public static new readonly List<string> RouteValueKeys = [..SubActionParameters.RouteValueKeys, "search", "page"];

    public string? Search { get; set; }
    public string? Page { get; set; }

    public override RouteValueDictionary AsRouteValues()
        => new(base.AsRouteValues().Merge(new { search = Search, page = Page }));
}
