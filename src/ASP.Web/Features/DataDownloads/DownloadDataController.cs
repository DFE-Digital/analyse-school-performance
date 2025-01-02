using ASP.Application;
using ASP.Application.UseCases.Downloads.GetAvailableDownloads;
using ASP.Application.UseCases.Downloads.DownloadAsZip;
using ASP.Core;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Utilities;
using ASP.Web.Core.BreadcrumbTrail;
using Microsoft.AspNetCore.Mvc;
using ASP.Core.DataDownloads;

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
    ///     <c>DownloadDataController</c> has a <c>HandleStep()</c> method that returns a <c>Result&lt;IActionResult&gt;</c> - 
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
    /// [HttpGet($"download-data/{DownloadDataController.SubRouteTemplate}")]
    /// [HttpPost($"download-data/{DownloadDataController.SubRouteTemplate}")]
    /// public Task<IActionResult> DownloadData(DownloadDataStepParameters parameters)
    /// {
    ///     var downloadData = new DownloadDataController(DataDownloadsScopeType.School, ControllerContext, Url, _api);
    ///     var result =
    ///     from urn in ...       // get school URN from somewhere
    ///     from otherData in ... // get other data needed to build the page
    ///     
    ///     // delegate to the DownloadDataController to handle validation, error cases etc 
    ///     // and call the showPageView function to render the page
    ///     from actionResult in downloadData.HandleStep(
    ///     parameters,    // step parameters model-bound from route values/query string parameters
    ///     urn,           // URN or LA code to look up available downloads
    ///
    ///     // base breadcrumb trail for the Download Data page to be added to 
    ///         this.GetDownloadDataBaseBreadcrumbTrail() depending on the download step
    ///
    ///         // showPageView function to render a page view 
    ///         // (displaying a download step, including any validation errors)
    ///         stepModel => View(
    ///         // view model for this page that includes the download data step view model
    ///             new DownloadDataPageViewModel {
    ///                 BreadcrumbTrail = stepModel.BreadcrumbTrail,
    ///                 PageTitle = stepModel.StepTitle,
    ///                 DownloadData = stepModel.DownloadData,
    ///                 // other view model properties needed to build the page
    ///             }))
    ///     select actionResult;
    ///
    ///     return result
    ///         .ToActionResult(_hostEnvironment);
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
    /// <partial name="~/Features/DataDownloads/_DownloadDataPartial.cshtml" model=@Model.DownloadData/>
    /// ]]>
    /// </code>
    /// </remarks>
    [NonController]
    public class DownloadDataController
    {
        public const string SubRouteTemplate = "{**step}";
        public static readonly RouteValueDictionary InitialRouteValues = new RouteValueDictionary(new { step = "", selectedYear = "", selectedFiles = "" });

        private readonly Dictionary<DownloadDataStepType, DownloadDataStep> _steps = new();

        public DownloadDataController(DataDownloadsScopeType scopeType, ControllerContext context, IUrlHelper url, IAspApiClient api, IDataDownloadsScopeValidator scopeValidator)
        {
            DownloadDataStep GetStep(DownloadDataStepType stepType) => _steps[stepType];

            _steps = new() {
                [DownloadDataStepType.SelectYear] =
                    new SelectYearStep("", "Dates available for download", scopeType, context, url, api, scopeValidator, GetStep),

                [DownloadDataStepType.SelectFiles] = 
                    new SelectFilesStep("select-files/", "Data files available for download", scopeType, context, url, api, scopeValidator, GetStep),

                [DownloadDataStepType.SelectFormat] = 
                    new SelectFormatStep("select-format/", "Download data", scopeType, context, url, api, scopeValidator, GetStep),

                [DownloadDataStepType.DownloadAsZip] = 
                    new DownloadAsZipStep("download-as-zip/", "Download as Zip file", scopeType, context, url, api, scopeValidator, GetStep)
            };
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
        public Task<Result<IActionResult>> HandleStep(
            DownloadDataStepParameters parameters,
            string scopeIdentifier,
            IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
            Func<DownloadDataStepViewModel, IActionResult> showPageView,
            Dictionary<DownloadDataStepType, DownloadDataStepConfig>? stepConfig = null
        )
        {
            // Step titles/paths can be overridden if needed
            if(stepConfig != null)
            {
                foreach((var key, var value) in stepConfig)
                {
                    if (value.Title != null)
                    {
                        _steps[key].Title = value.Title;
                    }

                    if (value.Path != null)
                    {
                        _steps[key].Path = value.Path;
                    }
                }
            }

            // Route value step in route template /path/to/download-data/{**step} is resolved as null if step portion is empty
            // so Step parameter will be null for first step, but we prefer to deal with it as an empty string
            var stepPath = parameters.Step ?? "";
            var step = _steps.Values.FirstOrDefault(step => step.Path == stepPath);

            if (step == null)
            {
                return Task.FromResult(Result.NotFound<IActionResult>($"Download data step \"{stepPath}\" was not recognised."));
            }

            return step.Handle(parameters, scopeIdentifier, baseBreadcrumbTrail, showPageView);
        }

        private abstract class DownloadDataStep
        {
            private readonly IDataDownloadsScopeValidator _scopeValidator;

            public string Path { get; set; }
            public string Title { get; set; }
            protected DataDownloadsScopeType ScopeType { get; }
            protected ControllerContext Context { get; }
            protected IUrlHelper Url { get; }
            protected IAspApiClient Api { get; }
            protected Func<DownloadDataStepType, DownloadDataStep> GetStep { get; }

            public DownloadDataStep(
                string path,
                string title,
                DataDownloadsScopeType scopeType,
                ControllerContext context,
                IUrlHelper url,
                IAspApiClient api,
                IDataDownloadsScopeValidator scopeValidator,
                Func<DownloadDataStepType, DownloadDataStep> getStep)
            {
                _scopeValidator = scopeValidator;
                
                Path = path;
                Title = title;
                ScopeType = scopeType;
                Context = context;
                Url = url;
                Api = api;
                GetStep = getStep;
            }

            public abstract object GetRouteValues(DownloadDataStepParameters parameters);

            public abstract Task<Result<IActionResult>> Handle(
                DownloadDataStepParameters parameters,
                string scopeIdentifier,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail, 
                Func<DownloadDataStepViewModel, IActionResult> showPageView
            );

            protected Task<Result<AvailableDownloadsViewModel>> GetAvailableDownloads(string scopeIdentifier, Optional<int> year)
            {
                return
                    from scope in _scopeValidator.ValidateScope(
                        ScopeType,
                        scopeIdentifier,
                        year
                    )
                    from downloads in Api.GetAvailableDownloads(new GetAvailableDownloadsRequest(scope.ScopeType, scope.Identifier, scope.Year))
                    select AvailableDownloadsViewModel.FromAvailableDownloads(downloads);
            }

            protected string Action(string action, object? values = null)
                => Url.Action(action, Context.ActionDescriptor.ControllerName, values)
                    ?? "";

            protected string Action(object values)
                => Url.Action(Context.ActionDescriptor.ActionName, Context.ActionDescriptor.ControllerName, values)
                    ?? "";

            protected Result<IActionResult> Redirect(object values)
                => new RedirectToActionResult(Context.ActionDescriptor.ActionName, Context.ActionDescriptor.ControllerName, values);
        };

        private class SelectYearStep : DownloadDataStep
        {
            public SelectYearStep(
                string path,
                string title,
                DataDownloadsScopeType scopeType,
                ControllerContext context,
                IUrlHelper url,
                IAspApiClient api,
                IDataDownloadsScopeValidator scopeValidator,
                Func<DownloadDataStepType, DownloadDataStep> getStep) 
                : base(path, title, scopeType, context, url, api, scopeValidator, getStep)
            {
            }

            public override object GetRouteValues(DownloadDataStepParameters parameters)
                => InitialRouteValues;

            public override async Task<Result<IActionResult>> Handle(
                DownloadDataStepParameters parameters,
                string scopeIdentifier,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<DownloadDataStepViewModel, IActionResult> showPageView)
            {
                if (Context.HttpContext.Request.Method == HttpMethods.Post)
                {
                    // Redirect to files selection if year is valid
                    if (parameters.SelectedYear.HasValue)
                    {
                        return Redirect(GetStep(DownloadDataStepType.SelectFiles).GetRouteValues(parameters));
                    }

                    Context.ModelState.AddModelError(Constants.ModelErrorKeySelectedYear,
                        Constants.AcademicYearToDownldValidationErrorMessage);
                }

                return
                    from availableDownloads in await GetAvailableDownloads(scopeIdentifier, Optional<int>.None)
                        .DefaultIf(e => e is NotFoundError, new())
                    let title = availableDownloads.Downloads.Any() 
                        ? Title
                        : "We could not find any data downloads"
                    let downloadDataModel = availableDownloads.Downloads.Any()
                        ? (DownloadDataViewModel)new DownloadDataSelectYearViewModel(availableDownloads.AvailableDates)
                        : new DownloadDataNoDownloadsAvailableViewModel(
                            ScopeType == DataDownloadsScopeType.LA ? "LA" : "school"
                          )
                    let model = new DownloadDataStepViewModel(
                        title,
                        new BreadcrumbTrailViewModel(
                            baseBreadcrumbTrail,
                            title
                        ),
                        downloadDataModel
                    )
                    select showPageView(model);
            }
        }

        private class SelectFilesStep : DownloadDataStep
        {
            public SelectFilesStep(
                string path,
                string title,
                DataDownloadsScopeType scopeType,
                ControllerContext context,
                IUrlHelper url,
                IAspApiClient api,
                IDataDownloadsScopeValidator scopeValidator,
                Func<DownloadDataStepType, DownloadDataStep> getStep) 
                : base(path, title, scopeType, context, url, api, scopeValidator, getStep)
            {
            }

            public override object GetRouteValues(DownloadDataStepParameters parameters)
                => new { step = Path.TrimEnd('/'), selectedYear = parameters.SelectedYear, selectedFiles = "" };

            public override async Task<Result<IActionResult>> Handle(
                DownloadDataStepParameters parameters,
                string scopeIdentifier,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<DownloadDataStepViewModel, IActionResult> showPageView)
            {
                if (Context.HttpContext.Request.Method == HttpMethods.Post)
                {
                    var selectedFiles = parameters.SelectedFiles ?? new();

                    // Redirect to data select format page if year is valid
                    if (selectedFiles.Any())
                    {
                        return Redirect(GetStep(DownloadDataStepType.SelectFormat).GetRouteValues(parameters));
                    }

                    Context.ModelState.AddModelError(Constants.ModelErrorKeySelectedFiles,
                        Constants.DataFilesAvialableForDownlodValidationErrorMessage);
                }

                var selectYearStep = GetStep(DownloadDataStepType.SelectYear);

                return
                    from availableDownloads in await GetAvailableDownloads(scopeIdentifier, Optional.FromNullable(parameters.SelectedYear))
                        .DefaultIf(e => e is NotFoundError, new())
                    let title = availableDownloads.Downloads.Any()
                        ? Title
                        : "We could not find any data downloads"
                    let downloadDataModel = availableDownloads.Downloads.Any()
                        ? (DownloadDataViewModel)new DownloadDataSelectFilesViewModel(availableDownloads.Downloads)
                        : new DownloadDataNoDownloadsAvailableViewModel(
                            ScopeType == DataDownloadsScopeType.LA ? "LA" : "school",
                            parameters.SelectedYear,
                            Action(GetStep(DownloadDataStepType.SelectYear).GetRouteValues(parameters))
                          )
                    let model = new DownloadDataStepViewModel(
                        title,
                        new BreadcrumbTrailViewModel(
                            baseBreadcrumbTrail.Concat([
                                new(selectYearStep.Title, Action(selectYearStep.GetRouteValues(parameters))),
                            ]),
                            title
                        ),
                        downloadDataModel
                    )
                    select showPageView(model);
            }
        }

        private class SelectFormatStep : DownloadDataStep
        {
            public SelectFormatStep(
                string path,
                string title,
                DataDownloadsScopeType scopeType,
                ControllerContext context,
                IUrlHelper url,
                IAspApiClient api,
                IDataDownloadsScopeValidator scopeValidator,
                Func<DownloadDataStepType, DownloadDataStep> getStep) 
                : base(path, title, scopeType, context, url, api, scopeValidator, getStep)
            {
            }

            public override object GetRouteValues(DownloadDataStepParameters parameters)
                => new { step = Path.TrimEnd('/'), selectedYear = parameters.SelectedYear, selectedFiles = parameters.SelectedFiles };

            public override Task<Result<IActionResult>> Handle(
                DownloadDataStepParameters parameters,
                string scopeIdentifier,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<DownloadDataStepViewModel, IActionResult> showPageView)
            {
                var selectYearStep = GetStep(DownloadDataStepType.SelectYear);
                var selectFilesStep = GetStep(DownloadDataStepType.SelectFiles);
                var downloadAsZipStep = GetStep(DownloadDataStepType.DownloadAsZip);

                var model = new DownloadDataStepViewModel(
                    Title,
                    new BreadcrumbTrailViewModel(
                        baseBreadcrumbTrail.Concat([
                            new(selectYearStep.Title, Action(selectYearStep.GetRouteValues(parameters))),
                            new(selectFilesStep.Title, Action(selectFilesStep.GetRouteValues(parameters)))
                        ]),
                        Title
                    ),
                    new DownloadDataSelectFormatViewModel(
                        ScopeType == DataDownloadsScopeType.LA ? "LA" : "school",
                        [("Data in CSV format", Action(downloadAsZipStep.GetRouteValues(parameters with { FileType = FileType.CSV })))],
                        Action(selectYearStep.GetRouteValues(parameters))
                    )
                );

                return Task.FromResult(Result.Success(showPageView(model)));
            }
        }

        private class DownloadAsZipStep : DownloadDataStep
        {
            public DownloadAsZipStep(
                string path,
                string title,
                DataDownloadsScopeType scopeType,
                ControllerContext context,
                IUrlHelper url,
                IAspApiClient api,
                IDataDownloadsScopeValidator scopeValidator,
                Func<DownloadDataStepType, DownloadDataStep> getStep)
                : base(path, title, scopeType, context, url, api, scopeValidator, getStep)
            {
            }

            public override object GetRouteValues(DownloadDataStepParameters parameters)
                => new { step = Path.TrimEnd('/'), fileType = parameters.FileType, selectedFiles = parameters.SelectedFiles };

            public override async Task<Result<IActionResult>> Handle(
                DownloadDataStepParameters parameters,
                string scopeIdentifier,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<DownloadDataStepViewModel, IActionResult> showPageView)
            {
                var fileType = parameters.FileType ?? FileType.CSV;
                var selectedFiles = parameters.SelectedFiles ?? new();

                return
                    from response in await Api.DownloadAsZipFile(new DownloadAsZipFileRequest(fileType, selectedFiles))
                    select (IActionResult)new FileStreamResult(response.Content, response.ContentType) {
                        FileDownloadName = response.FileName
                    };
            }
        }
    }
}