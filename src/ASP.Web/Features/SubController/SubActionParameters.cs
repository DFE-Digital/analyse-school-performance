namespace ASP.Web.Features.SubController;

public abstract record SubActionParameters
{
    public static readonly List<string> RouteValueKeys = ["subAction"];

    public string? SubAction { get; set; }

    public virtual RouteValueDictionary AsRouteValues()
        => new(new { subAction = SubAction });
}