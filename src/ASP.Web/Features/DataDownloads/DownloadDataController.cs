using ASP.Application;
using ASP.Application.UseCases.Downloads.GetAvailableDownloads;
using ASP.Application.UseCases.Downloads.GetDownloadPackage;
using ASP.Core;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Utilities;
using ASP.Web.Core.BreadcrumbTrail;
using Microsoft.AspNetCore.Mvc;
using ASP.Core.DataDownloads;
using ASP.Web.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ASP.Web.Features.DataDownloads
{
    /// <summary>
    /// <para>
    ///     <c>DownloadDataController</c> is a sub-controller that can be hosted within a controller action, 
    ///     allowing its routing and authorization to be determined by the hosting controller. 
    ///     The controller action specifies its route using attribute routing, but then delegates to the `DownloadDataController` 
    ///     to resolve and handle the download step based on the sub-route and query string parameters. 
    /// </para>
    /// </summary>
    /// 
    /// <remarks>
    /// <para>The action needs to handle GET and POST methods:</para>
    /// <code>
    /// <![CDATA[
    /// // Adds download step as a catch-all route parameter to the route template e.g. "download-data/{**step}"
    /// [HttpGet($"download-data/{DownloadDataController.SubRouteTemplate}")]
    /// [HttpPost($"download-data/{DownloadDataController.SubRouteTemplate}")]
    /// public Task<IActionResult> DownloadData(DownloadDataStepParameters parameters)
    /// {
    ///     ...
    /// }
    /// ]]>
    /// </code>
    /// <para>
    ///     <c>DownloadDataController</c> has a <c>Handle()</c> method that returns a <c>Result&lt;IActionResult&gt;</c> - 
    ///     this can then be incorporated in a <c>Result&lt;&gt;</c> chain along with other <c>Result&lt;&gt;</c> actions 
    ///     that may be needed to build up the page.
    /// </para>
    /// <para>
    ///     This needs to be passed a base breadcrumb trail that can be added to as the download steps are navigated, and a 
    ///     <c>showPageView</c> function to be called to render the page. This is so that the <c>DownloadDataController</c> 
    ///     can take control of handling the download step logic, including validation errors, not found errors (if the step part 
    ///     of the route is invalid), and returning a file download response, rendering the page by calling the 
    ///     <c>showPageView</c> function as needed.
    /// </para>
    /// <code>
    /// <![CDATA[
    /// public class SchoolController : Controller
    /// {
    ///     private readonly DownloadDataController _downloadDataController;
    ///     
    ///     public class SchoolController(IAspApiClient api, IDataDownloadsScopeValidator scopeValidator) 
    ///     {
    ///         _downloadDataController = new DownloadDataController(
    ///             "DownloadData",                // Action that will "host" the download steps
    ///             "School",                      // "Host" controller (i.e. this controller)
    ///             [],                            // Keys of any route values the "host" action requires
    ///             DataDownloadsScopeType.School, // Whether the downloads are School or LA downloads
    ///             api,                           // API client instance
    ///             scopeValidator);               // Validator to validate DataDownloadScope
    ///     }
    ///     
    ///     public override void OnActionExecuting(ActionExecutingContext context)
    ///     {
    ///         base.OnActionExecuting(context);
    ///
    ///         // Give the controller access to the currently executing ActionContext 
    ///         // in order to access route values and be able to redirect to sub-actions
    ///         _downloadDataController.BindContext(context);
    ///     }
    ///
    ///     [HttpGet($"download-data/{DownloadDataController.SubRouteTemplate}")]
    ///     [HttpPost($"download-data/{DownloadDataController.SubRouteTemplate}")]
    ///     public Task<IActionResult> DownloadData(DownloadDataStepParameters parameters)
    ///     {
    ///         var result =
    ///             from urn in ...       // get school URN from somewhere
    ///             from otherData in ... // get other data needed to build the page
    ///         
    ///             // delegate to the DownloadDataController to handle validation, error cases etc 
    ///             // and call the showPageView function to render the page
    ///             from actionResult in _downloadDataController.Handle(
    ///                 parameters,    // step parameters model-bound from route values/query string parameters
    ///                 urn,           // URN or LA code to look up available downloads
    ///                 
    ///                 // base breadcrumb trail for the Download Data page to be added to 
    ///                 // depending on the download step
    ///                 this.GetDownloadDataBaseBreadcrumbTrail() 
    ///                 
    ///                 // showPageView function to render a page view 
    ///                 // (displaying a download step, including any validation errors)
    ///                 model => View(
    ///                     // view model for this page that includes the download data step view model
    ///                     new DownloadDataPageViewModel {
    ///                         BreadcrumbTrail = model.BreadcrumbTrail,
    ///                         PageTitle = model.StepTitle,
    ///                         DownloadData = model.DownloadData,
    /// 
    ///                         // other view model properties needed to build the page
    ///                     }))
    ///             select actionResult;
    ///    
    ///         return result
    ///             .ToActionResult(_hostEnvironment);
    ///     }
    /// }
    /// ]]>
    /// </code>
    ///
    /// <para>
    ///     On the page view side, the <c>Features/DataDownloads/_DownloadDataPartial</c> just needs to be dropped into the view 
    ///     wherever the download data step page should be rendered. For example here's a dummy page view:
    /// </para>
    /// 
    /// <code>
    /// <![CDATA[
    /// @model DownloadDataPageViewModel
    /// <partial name="_BreadcrumbTrail" model=@Model.BreadcrumbTrail />
    /// <h1>@Model.PageTitle</h1>
    /// /* ... other stuff */
    /// <partial name="Features/DataDownloads/_DownloadDataPartial" model=@Model.DownloadData/>
    /// ]]>
    /// </code>
    /// </remarks>
    [NonController]
    public class DownloadDataController
    {
        public const string SubRouteTemplate = "{**step}";

        private readonly string _hostAction;
        private readonly string _hostController;
        private readonly List<string> _hostActionRouteValueKeys;
        private readonly DataDownloadScopeType _scopeType;
        private readonly IAspApiClient _api;
        private readonly IDataDownloadsScopeValidator _scopeValidator;
        private readonly Dictionary<DownloadDataStepType, DownloadDataStep> _steps = new();
        private RouteValueDictionary? _hostActionRouteValues;
        private ActionContext? _context;
        private IUrlHelper? _urlHelper;

        /// <summary>
        /// Creates a <c>DownloadDataController</c> instance to handle data downloads.
        /// </summary>
        /// <param name="hostAction">Action that will "host" the search sub-actions</param>
        /// <param name="hostController">Controller the "host" action belongs to</param>
        /// <param name="hostActionRouteValueKeys">Keys of any route values the host action requires</param>
        /// <param name="scopeType">Whether the downloads are School or LA downloads</param>
        /// <param name="api">API client instance</param>
        /// <param name="scopeValidator">Validator to validate DataDownloadScope</param>
        /// <param name="stepRouteConfig">Optional config to override the default paths for each step</param>
        public DownloadDataController(
            string hostAction,
            string hostController,
            List<string> hostActionRouteValueKeys,
            DataDownloadScopeType scopeType,
            IAspApiClient api,
            IDataDownloadsScopeValidator scopeValidator,
            Dictionary<DownloadDataStepType, DownloadDataStepRouteConfig>? stepRouteConfig = null)
        {
            _hostAction = hostAction;
            _hostController = hostController;
            _hostActionRouteValueKeys = hostActionRouteValueKeys;
            _scopeType = scopeType;
            _api = api;
            _scopeValidator = scopeValidator;

            _steps = new()
            {
                [DownloadDataStepType.SelectYear] =
                    new SelectYearStep(this, "", "Dates available for download"),

                [DownloadDataStepType.SelectFiles] =
                    new SelectFilesStep(this, "select-files", "Data files available for download"),

                [DownloadDataStepType.SelectFormat] =
                    new SelectFormatStep(this, "select-format", "Download data"),

                [DownloadDataStepType.DownloadAsZip] =
                    new DownloadAsZipStep(this, "download-as-zip", "Download as Zip file")
            };

            // Step paths can be overridden if needed
            if (stepRouteConfig != null)
            {
                foreach ((var key, var value) in stepRouteConfig)
                {
                    if (value.Path != null)
                    {
                        _steps[key].Path = value.Path.TrimEnd('/', ' ');
                    }
                }
            }
        }

        /// <summary>
        /// This method should be called from OnActionExecuting() as the controller needs access to the currently executing
        /// ActionContext in order to access route values and be able to redirect to sub-actions.
        /// </summary>
        /// <param name="context"></param>
        public void BindContext(ActionContext context)
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
        public string GetInitialActionUrl(object? routeValues = null)
        {
            var stepRouteValues = GetRouteValuesForStep(DownloadDataStepType.SelectYear, new DownloadDataStepParameters(), routeValues ?? new { });
            var action = _urlHelper?.Action(_hostAction, _hostController, stepRouteValues);
            return action ?? "";
        }

        /// <summary>
        /// <para>
        ///     Handles the download step logic, including validation errors, not found errors (if the step part of the route is invalid), 
        ///     and returning a file download response, rendering the page by calling the <c>showPageView</c> function as needed.
        /// </para>
        /// </summary>
        /// <param name="parameters">Download data step parameters model-bound from route values/query string parameters</param>
        /// <param name="scopeIdentifier">URN or LA code to look up available downloads</param>
        /// <param name="baseBreadcrumbTrail">Base breadcrumb trail for the page that can be added to as the download steps are navigated</param>
        /// <param name="showPageView">Function to render a page view (displaying a download step, including any validation errors) based on a <c>DownloadDataStepModel</c></param>
        /// <param name="stepConfig">Dictionary of overrides for any steps that need to be given different titles or paths</param>
        /// <returns>a <c>Result&lt;IActionResult&gt;</c> that can then be incorporated in a <c>Result&lt;&gt;</c> chain along with other <c>Result&lt;&gt;</c> actions that may be needed to build up the page</returns>
        public Task<Result<IActionResult>> Handle(
            DownloadDataStepParameters parameters,
            string scopeIdentifier,
            IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
            Func<DownloadDataStepViewModel, IActionResult> showPageView,
            Dictionary<DownloadDataStepType, DownloadDataStepConfig>? stepConfig = null
        )
        {
            // Step titles can be overridden if needed
            if (stepConfig != null)
            {
                foreach ((var key, var value) in stepConfig)
                {
                    if (value.Title != null)
                    {
                        _steps[key].Title = value.Title;
                    }
                }
            }

            // Route value step in route template /path/to/download-data/{**step} is resolved as null if step portion is empty
            // so Step parameter will be null for first step, but we prefer to deal with it as an empty string
            var stepPath = (parameters.Step ?? "").TrimEnd('/', ' ');
            var step = _steps.Values.FirstOrDefault(step => step.Path == stepPath);

            if (step == null)
            {
                return Task.FromResult(Result.NotFound<IActionResult>($"Download data step \"{stepPath}\" was not recognised."));
            }

            return step.Handle(parameters, scopeIdentifier, baseBreadcrumbTrail, showPageView);
        }

        private string? RequestMethod
            => _context?.HttpContext.Request.Method;

        private string ScopeTypeLabel
            => _scopeType == DataDownloadScopeType.LA ? "LA" : "school";

        private void AddModelError(string key, string errorMessage)
            => _context?.ModelState.AddModelError(key, errorMessage);

        private Result<IActionResult> RedirectToStep(DownloadDataStepType stepType, DownloadDataStepParameters parameters)
            => new RedirectToActionResult(_hostAction, _hostController, GetRouteValuesForStep(stepType, parameters));

        private BreadcrumbItem GetBreadcrumbForStep(DownloadDataStepType stepType, DownloadDataStepParameters parameters)
        {
            var step = _steps[stepType];
            return new BreadcrumbItem(step.Title, GetUrlForStep(stepType, parameters));
        }

        private string GetUrlForStep(DownloadDataStepType stepType, DownloadDataStepParameters parameters)
            => _urlHelper?.Action(_hostAction, _hostController, GetRouteValuesForStep(stepType, parameters)) ?? "";

        private RouteValueDictionary GetRouteValuesForStep(DownloadDataStepType stepType, DownloadDataStepParameters parameters)
            => (_hostActionRouteValues ?? [])
                .Merge(_steps[stepType].GetRouteValues(parameters));

        private RouteValueDictionary GetRouteValuesForStep(DownloadDataStepType stepType, DownloadDataStepParameters parameters, object initialRouteValues)
            => (_hostActionRouteValues ?? [])
                .Merge(initialRouteValues)
                .Merge(_steps[stepType].GetRouteValues(parameters));

        private Task<Result<AvailableDownloadsViewModel>> GetAvailableDownloads(string scopeIdentifier, Optional<int> year)
        {
            return
                from scope in _scopeValidator.ValidateScope(
                    _scopeType,
                    scopeIdentifier,
                    year
                )
                from downloads in _api.GetAvailableDownloads(new GetAvailableDownloadsRequest(scope.ScopeType, scope.Identifier, scope.Year))
                select AvailableDownloadsViewModel.FromAvailableDownloads(downloads);
        }

        private class SelectYearStep : DownloadDataStep
        {
            public SelectYearStep(
                DownloadDataController controller,
                string path,
                string title)
                : base(controller, path, title)
            {
            }

            public override object GetRouteValues(DownloadDataStepParameters parameters)
                => new { step = Path, selectedYear = "", selectedFiles = "" };

            public override async Task<Result<IActionResult>> Handle(
                DownloadDataStepParameters parameters,
                string scopeIdentifier,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<DownloadDataStepViewModel, IActionResult> showPageView)
            {
                if (Controller.RequestMethod == HttpMethods.Post)
                {
                    // Redirect to files selection if year is valid
                    if (parameters.SelectedYear.HasValue)
                    {
                        return Controller.RedirectToStep(DownloadDataStepType.SelectFiles, parameters);
                    }

                    Controller.AddModelError(Constants.ModelErrorKeySelectedYear,
                        Constants.AcademicYearToDownldValidationErrorMessage);
                }

                return
                    from availableDownloads in await Controller.GetAvailableDownloads(scopeIdentifier, Optional<int>.None)
                        .DefaultIf(e => e is NotFoundError, new())
                    let title = availableDownloads.Downloads.Any()
                        ? Title
                        : "We could not find any data downloads"
                    let downloadDataModel = availableDownloads.Downloads.Any()
                        ? (DownloadDataViewModel)new DownloadDataSelectYearViewModel(
                            availableDownloads.AvailableDates
                          )
                        : new DownloadDataNoDownloadsAvailableViewModel(
                            Controller.ScopeTypeLabel
                          )
                    let model = new DownloadDataStepViewModel(
                        title,
                        new BreadcrumbTrailViewModel(
                            baseBreadcrumbTrail
                        ),
                        downloadDataModel
                    )
                    select showPageView(model);
            }
        }

        private class SelectFilesStep : DownloadDataStep
        {
            public SelectFilesStep(
                DownloadDataController controller,
                string path,
                string title)
                : base(controller, path, title)
            {
            }

            public override object GetRouteValues(DownloadDataStepParameters parameters)
                => new { step = Path, selectedYear = parameters.SelectedYear, selectedFiles = "" };

            public override async Task<Result<IActionResult>> Handle(
                DownloadDataStepParameters parameters,
                string scopeIdentifier,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<DownloadDataStepViewModel, IActionResult> showPageView)
            {
                if (Controller.RequestMethod == HttpMethods.Post)
                {
                    var selectedFiles = parameters.SelectedFiles ?? new();

                    // Redirect to data select format page if year is valid
                    if (selectedFiles.Any())
                    {
                        return Controller.RedirectToStep(DownloadDataStepType.SelectFormat, parameters);
                    }

                    Controller.AddModelError(Constants.ModelErrorKeySelectedFiles,
                        Constants.DataFilesAvialableForDownlodValidationErrorMessage);
                }

                return
                    from availableDownloads in await Controller.GetAvailableDownloads(scopeIdentifier, Optional.FromNullable(parameters.SelectedYear))
                        .DefaultIf(e => e is NotFoundError, new())
                    let title = availableDownloads.Downloads.Any()
                        ? Title
                        : "We could not find any data downloads"
                    let downloadDataModel = availableDownloads.Downloads.Any()
                        ? (DownloadDataViewModel)new DownloadDataSelectFilesViewModel(
                            availableDownloads.Downloads)
                        : new DownloadDataNoDownloadsAvailableViewModel(
                            Controller.ScopeTypeLabel,
                            parameters.SelectedYear,
                            Controller.GetUrlForStep(DownloadDataStepType.SelectYear, parameters))
                    let model = new DownloadDataStepViewModel(
                        title,
                        new BreadcrumbTrailViewModel(
                            baseBreadcrumbTrail.Concat([
                                Controller.GetBreadcrumbForStep(DownloadDataStepType.SelectYear, parameters)
                            ])
                        ),
                        downloadDataModel
                    )
                    select showPageView(model);
            }
        }

        private class SelectFormatStep : DownloadDataStep
        {
            public SelectFormatStep(
                DownloadDataController controller,
                string path,
                string title)
                : base(controller, path, title)
            {
            }

            public override object GetRouteValues(DownloadDataStepParameters parameters)
                => new { step = Path, selectedYear = parameters.SelectedYear, selectedFiles = parameters.SelectedFiles };

            public override Task<Result<IActionResult>> Handle(
                DownloadDataStepParameters parameters,
                string scopeIdentifier,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<DownloadDataStepViewModel, IActionResult> showPageView)
            {

                List<(string, string?)> fileFormatList = [];

                foreach (FileType fileType in Enum.GetValues<FileType>())
                {
                    fileFormatList.Add(($"Data in {fileType} format", Controller.GetUrlForStep(DownloadDataStepType.DownloadAsZip, parameters with { FileType = fileType })));
                }

                var model = new DownloadDataStepViewModel(
                    Title,
                    new BreadcrumbTrailViewModel(
                        baseBreadcrumbTrail.Concat([
                            Controller.GetBreadcrumbForStep(DownloadDataStepType.SelectYear, parameters),
                            Controller.GetBreadcrumbForStep(DownloadDataStepType.SelectFiles, parameters)
                        ])
                    ),
                    new DownloadDataSelectFormatViewModel(
                        Controller.ScopeTypeLabel,
                        fileFormatList,
                        Controller.GetUrlForStep(DownloadDataStepType.SelectYear, parameters)
                    )
                );

                return Task.FromResult(Result.Success(showPageView(model)));
            }
        }

        private class DownloadAsZipStep : DownloadDataStep
        {
            public DownloadAsZipStep(
                DownloadDataController controller,
                string path,
                string title)
                : base(controller, path, title)
            {
            }

            public override object GetRouteValues(DownloadDataStepParameters parameters)
                => new { step = Path, fileType = parameters.FileType, selectedFiles = parameters.SelectedFiles };

            public override async Task<Result<IActionResult>> Handle(
                DownloadDataStepParameters parameters,
                string scopeIdentifier,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<DownloadDataStepViewModel, IActionResult> showPageView)
            {
                var fileType = parameters.FileType ?? FileType.CSV;
                var selectedFiles = parameters.SelectedFiles ?? new();

                return
                    from response in await Controller._api.GetDownloadPackage(new GetDownloadPackageRequest(fileType, selectedFiles, Controller._scopeType, scopeIdentifier))
                    select (IActionResult)new FileStreamResult(response.Content, response.ContentType)
                    {
                        FileDownloadName = response.FileName
                    };
            }
        }

        private abstract class DownloadDataStep
        {
            public DownloadDataController Controller { get; }
            public string Path { get; set; }
            public string Title { get; set; }

            public DownloadDataStep(
                DownloadDataController controller,
                string path,
                string title)
            {
                Controller = controller;
                Path = path;
                Title = title;
            }

            public abstract object GetRouteValues(DownloadDataStepParameters parameters);

            public abstract Task<Result<IActionResult>> Handle(
                DownloadDataStepParameters parameters,
                string scopeIdentifier,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<DownloadDataStepViewModel, IActionResult> showPageView
            );
        };
    }
}