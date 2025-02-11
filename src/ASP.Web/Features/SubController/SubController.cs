using ASP.Core.Results;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ASP.Web.Features.SubController;

/// <summary>
/// <para>
///     <c>SubController</c> represents the concept of a sub-controller that can be hosted within a controller action,
///     allowing its routing and authorization to be determined by the hosting controller. 
///     The controller action specifies its route using attribute routing, but then delegates to the `SearchController` 
///     to resolve and handle the search sub-action based on the sub-route and query string parameters. 
/// </para>
/// </summary>
/// <typeparam name="TController">Type of the derived controller inheriting from this class.</typeparam>
/// <typeparam name="TParameters">Type of the parameters object model-bound to the route values of the host action.</typeparam>
/// <typeparam name="TScope">Custom type representing a scope that the sub-action occurs within (e.g. EstablishmentScope)</typeparam>
/// <typeparam name="TSubActionType">Enum type enumerating the distinct sub-actions handled by the sub-controller</typeparam>
/// <typeparam name="TSubActionViewModel">Base view model that all sub-actions share.</typeparam>
[NonController]
public abstract class SubController<TController, TParameters, TScope, TSubActionType, TSubActionViewModel>
    where TController : SubController<TController, TParameters, TScope, TSubActionType, TSubActionViewModel>
    where TSubActionType : struct, Enum
    where TParameters : SubActionParameters, new()
{
    public const string SubRouteTemplate = "{**subAction}";

    private readonly string _hostAction;
    private readonly string _hostController;
    private readonly List<string> _hostActionRouteValueKeys;
    private readonly Dictionary<TSubActionType, SubAction> _subActions = new();
    private readonly Dictionary<TSubActionType, SubActionRouteConfig>? _subActionRouteConfig = null;
    private RouteValueDictionary? _hostActionRouteValues;
    
    protected ActionContext? _context;
    protected IUrlHelper? _urlHelper;

    public SubController(
        string hostAction,
        string hostController,
        List<string> hostActionRouteValueKeys,
        Dictionary<TSubActionType, SubActionRouteConfig>? subActionRouteConfig = null)
    {
        _hostAction = hostAction;
        _hostController = hostController;
        _hostActionRouteValueKeys = hostActionRouteValueKeys;
        _subActionRouteConfig = subActionRouteConfig;
    }

    protected void AddSubAction(TSubActionType subActionType, SubAction subAction)
    {
        _subActions[subActionType] = subAction;

        // Sub action paths can be overridden if needed
        if (_subActionRouteConfig != null &&
            _subActionRouteConfig.TryGetValue(subActionType, out SubActionRouteConfig? config) &&
            config != null &&
            config.Path != null)
        {
            _subActions[subActionType].Path = config.Path.TrimEnd('/', ' ');
        }
    }

    /// <summary>
    /// This method should be called from OnActionExecuting() as the controller needs access to the currently executing
    /// ActionContext in order to access route values and be able to redirect to sub-actions.
    /// </summary>
    /// <param name="context"></param>
    public virtual void BindContext(ActionContext context)
    {
        _context = context;
        var factory = context.HttpContext?.RequestServices?.GetRequiredService<IUrlHelperFactory>();
        _urlHelper = factory?.GetUrlHelper(context);
        _hostActionRouteValues = new RouteValueDictionary(
            _context.RouteData.Values.Where(kvp => _hostActionRouteValueKeys.Contains(kvp.Key)));
    }

    /// <summary>
    /// Gets the Url for the initial action of this controller, using the optional routeValues provided
    /// </summary>
    /// <param name="routeValues">Optional routeValues for the initial action</param>
    /// <returns></returns>
    public virtual string GetInitialActionUrl(object? routeValues = null)
    {
        var subActionRouteValues = GetRouteValuesForSubAction(Enum.GetValues<TSubActionType>()[0], new TParameters(), routeValues ?? new { });
        var action = _urlHelper?.Action(_hostAction, _hostController, subActionRouteValues);
        return action ?? "";
    }

    /// <summary>
    /// <para>
    ///     Handles the sub-action logic, including validation errors, not found errors (if the subAction part of the route is invalid), 
    ///     and returning an IActionResult, rendering the page by calling the <c>showPageView</c> function as needed.
    /// </para>
    /// </summary>
    /// <param name="scope">Custom scope object</param>
    /// <param name="parameters">Sub-action parameters model-bound from route values/query string parameters</param>
    /// <param name="baseBreadcrumbTrail">Base breadcrumb trail for the page that can be added to as the sub-actions are navigated</param>
    /// <param name="showPageView">Function to render a page view (displaying a sub-action, including any validation errors) based on a <c>TSubActionViewModel</c></param>
    /// <param name="subActionConfig">Dictionary of overrides for any sub-actions that need to be given different titles/subtitles etc</param>
    /// <returns>a <c>Result&lt;IActionResult&gt;</c> that can then be incorporated in a <c>Result&lt;&gt;</c> chain along with other <c>Result&lt;&gt;</c> actions that may be needed to build up the page</returns>
    public virtual Task<Result<IActionResult>> Handle(
        TScope scope,
        TParameters parameters,
        IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
        Func<TSubActionViewModel, IActionResult> showPageView,
        Dictionary<TSubActionType, SubActionConfig>? subActionConfig = null
    )
    {
        // Sub action titles/subtitles can be overridden if needed
        if (subActionConfig != null)
        {
            foreach ((var key, var value) in subActionConfig)
            {
                if (value.Title != null)
                {
                    _subActions[key].Title = value.Title;
                }

                if (value.Subtitle != null)
                {
                    _subActions[key].Subtitle = $"{value.Subtitle} - ";
                }
            }
        }

        // Route value sub-action in route template /path/to/host/action/{**subAction} is resolved as null if subAction portion is empty
        // so SubAction parameter will be null for first sub-action, but we prefer to deal with it as an empty string
        var subActionPath = (parameters.SubAction ?? "").TrimEnd('/', ' ');
        var subAction = _subActions.Values.FirstOrDefault(a => a.Path == subActionPath);

        if (subAction == null)
        {
            return Task.FromResult(Result.NotFound<IActionResult>($"Sub-action \"{subActionPath}\" was not recognised."));
        }

        return subAction.Handle(parameters, scope, baseBreadcrumbTrail, showPageView);
    }

    protected string? RequestMethod
        => _context?.HttpContext.Request.Method;

    protected bool ModelStateIsValid
        => _context?.ModelState.IsValid ?? false;

    protected void AddModelError(string key, string errorMessage)
        => _context?.ModelState.AddModelError(key, errorMessage);

    protected Result<IActionResult> RedirectToSubAction(TSubActionType subActionType, TParameters parameters)
        => new RedirectToActionResult(_hostAction, _hostController, _subActions[subActionType].GetRouteValues(parameters));

    protected BreadcrumbItem GetBreadcrumbForSubAction(TSubActionType subActionType, TParameters parameters)
        => new BreadcrumbItem(_subActions[subActionType].Title, GetUrlForSubAction(subActionType, parameters));

    protected string GetUrlForSubAction(TSubActionType subActionType, TParameters parameters)
        => _urlHelper?.Action(_hostAction, _hostController, _subActions[subActionType].GetRouteValues(parameters)) ?? "";

    protected RouteValueDictionary GetRouteValuesForSubAction(TSubActionType subActionType, TParameters parameters)
        => (_hostActionRouteValues ?? [])
            .Merge(_subActions[subActionType].GetRouteValues(parameters));

    protected RouteValueDictionary GetRouteValuesForSubAction(TSubActionType subActionType, TParameters parameters, object initialRouteValues)
        => (_hostActionRouteValues ?? [])
            .Merge(initialRouteValues)
            .Merge(_subActions[subActionType].GetRouteValues(parameters));

    protected bool RequestContainsQueryStringKey(string key)
        => (_context?.HttpContext.Request.Query.Keys ?? [])
            .Any(k => k.Equals(key, StringComparison.InvariantCultureIgnoreCase));

    protected abstract class SubAction
    {
        public TController Controller { get; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }

        public SubAction(
            TController controller,
            string path,
            string title,
            string subtitle)
        {
            Controller = controller;
            Path = path;
            Title = title;
            Subtitle = subtitle;
        }

        public virtual RouteValueDictionary GetRouteValues(TParameters parameters)
            => new RouteValueDictionary(new { subAction = Path });

        public RouteValueDictionary GetRouteValues(TParameters parameters, object initialRouteValues)
            => new RouteValueDictionary(initialRouteValues).Merge(GetRouteValues(parameters));

        public abstract Task<Result<IActionResult>> Handle(
            TParameters parameters,
            TScope scope,
            IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
            Func<TSubActionViewModel, IActionResult> showPageView
        );
    }
}