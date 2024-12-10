using ASP.Application;
using ASP.Application.UseCases.Downloads.GetAvailableLADownloads;
using ASP.Application.UseCases.Downloads.DownloadAsZip;
using ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads;
using ASP.Core;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Utilities;
using ASP.Web.Core.BreadcrumbTrail;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Features.DataDownloads
{
    /// <summary>
    /// <para>
    ///     <c>DownloadDataStepController</c> is a sub-controller that can be hosted within a controller action, 
    ///     allowing its routing and authorization to be determined by the hosting controller. 
    ///     The controller action specifies its route using attribute routing, but then delegates to the `DownloadDataStepController` 
    ///     to resolve and handle the download step based on the sub-route and query string parameters. 
    /// </para>
    /// </summary>
    /// 
    /// <remarks>
    /// <para>The action needs to handle GET and POST methods:</para>
    /// <code>
    /// <![CDATA[
    /// // Adds download step as a catch-all route parameter to the route template e.g. "download-data/{**step}"
    /// [HttpGet($"download-data/{DownloadDataStepController.SubRouteTemplate}")]
    /// [HttpPost($"download-data/{DownloadDataStepController.SubRouteTemplate}")]
    /// public Task<IActionResult> DownloadData(DownloadDataStepParameters parameters)
    /// {
    ///     ...
    /// }
    /// ]]>
    /// </code>
    /// <para>
    ///     <c>DownloadDataStepController</c> has a <c>HandleStep()</c> method that returns a <c>Result&lt;IActionResult&gt;</c> - 
    ///     this can then be incorporated in a <c>Result&lt;&gt;</c> chain along with other <c>Result&lt;&gt;</c> actions 
    ///     that may be needed to build up the page.
    /// </para>
    /// <para>
    ///     This needs to be passed a base breadcrumb trail that can be added to as the download steps are navigated, and a 
    ///     <c>showPageView</c> function to be called to render the page. This is so that the <c>DownloadDataStepController</c> 
    ///     can take control of handling the download step logic, including validation errors, not found errors (if the step part 
    ///     of the route is invalid), and returning a file download response, rendering the page by calling the 
    ///     <c>showPageView</c> function as needed.
    /// </para>
    /// <code>
    /// <![CDATA[
    /// [HttpGet($"download-data/{DownloadDataStepController.SubRouteTemplate}")]
    /// [HttpPost($"download-data/{DownloadDataStepController.SubRouteTemplate}")]
    /// public Task<IActionResult> DownloadData(DownloadDataStepParameters parameters)
    /// {
    ///     var downloadData = new DownloadDataStepController(DownloadDataScope.School, ControllerContext, Url, _api);
    ///     var result =
    ///     from urn in ...       // get school URN from somewhere
    ///     from otherData in ... // get other data needed to build the page
    ///     
    ///     // delegate to the DownloadDataStepController to handle validation, error cases etc 
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
    public class DownloadDataStepController
    {
        public const string SubRouteTemplate = "{**step}";
        public static readonly RouteValueDictionary InitialRouteValues = new RouteValueDictionary(new { step = "", selectedYear = "", selectedFiles = "" });

        private readonly Dictionary<DownloadDataStepType, DownloadDataStep> _steps = new();

        public DownloadDataStepController(DownloadDataScope scope, ControllerContext context, IUrlHelper url, IAspApiClient api)
        {
            DownloadDataStep GetStep(DownloadDataStepType stepType) => _steps[stepType];

            _steps = new() {
                [DownloadDataStepType.SelectYear] =
                    new SelectYearStep("", "Dates available for download", scope, context, url, api, GetStep),

                [DownloadDataStepType.SelectFiles] = 
                    new SelectFilesStep("select-files/", "Data files available for download", scope, context, url, api, GetStep),

                [DownloadDataStepType.SelectFormat] = 
                    new SelectFormatStep("select-format/", "Download data", scope, context, url, api, GetStep),

                [DownloadDataStepType.DownloadAsZip] = 
                    new DownloadAsZipStep("download-as-zip/", "Download as Zip file", scope, context, url, api, GetStep)
            };
        }

        /// <summary>
        /// <para>
        ///     Handles the download step logic, including validation errors, not found errors (if the step part of the route is invalid), 
        ///     and returning a file download response, rendering the page by calling the <c>showPageView</c> function as needed.
        /// </para>
        /// </summary>
        /// <param name="parameters">Download data step parameters model-bound from route values/query string parameters</param>
        /// <param name="urnOrLACode">URN or LA code to look up available downloads</param>
        /// <param name="baseBreadcrumbTrail">Base breadcrumb trail for the page that can be added to as the download steps are navigated</param>
        /// <param name="showPageView">Function to render a page view (displaying a download step, including any validation errors) based on a <c>DownloadDataStepModel</c></param>
        /// <param name="stepTitleOverrides">Dictionary of overrides for titles of any steps that need to be given different titles</param>
        /// <returns>a <c>Result&lt;IActionResult&gt;</c> that can then be incorporated in a <c>Result&lt;&gt;</c> chain along with other <c>Result&lt;&gt;</c> actions that may be needed to build up the page</returns>
        public Task<Result<IActionResult>> HandleStep(
            DownloadDataStepParameters parameters,
            string urnOrLACode,
            IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
            Func<DownloadDataStepViewModel, IActionResult> showPageView,
            Dictionary<DownloadDataStepType, string>? stepTitleOverrides = null
        )
        {
            // Step titles can be overridden if needed
            if(stepTitleOverrides != null)
            {
                foreach((var key, var value) in stepTitleOverrides)
                {
                    _steps[key].Title = value;
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

            return step.Handle(parameters, urnOrLACode, baseBreadcrumbTrail, showPageView);
        }

        private abstract class DownloadDataStep
        {
            public string Path { get; }
            public string Title { get; set; }
            protected DownloadDataScope Scope { get; }
            protected ControllerContext Context { get; }
            protected IUrlHelper Url { get; }
            protected IAspApiClient Api { get; }
            protected Func<DownloadDataStepType, DownloadDataStep> GetStep { get; }

            public DownloadDataStep(string path,
                string title,
                DownloadDataScope scope,
                ControllerContext context,
                IUrlHelper url,
                IAspApiClient api,
                Func<DownloadDataStepType, DownloadDataStep> getStep)
            {
                Path = path;
                Title = title;
                Scope = scope;
                Context = context;
                Url = url;
                Api = api;
                GetStep = getStep;
            }

            public abstract object GetRouteValues(DownloadDataStepParameters parameters);

            public abstract Task<Result<IActionResult>> Handle(
                DownloadDataStepParameters parameters,
                string urnOrLACode,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail, 
                Func<DownloadDataStepViewModel, IActionResult> showPageView
            );

            protected Task<Result<AvailableDownloadsViewModel>> GetAvailableDownloads(string urnOrlaCode, Optional<int> year)
            {
                return Scope == DownloadDataScope.LocalAuthority

                    ? from downloads in Api.GetAvailableLaDownloads(new GetAvailableLADownloadsRequest(urnOrlaCode, year))
                      select AvailableDownloadsViewModel.FromAvailableLADownloads(downloads)

                    : from downloads in Api.GetAvailableSchoolDownloads(new GetAvailableSchoolDownloadsRequest(urnOrlaCode, year))
                      select AvailableDownloadsViewModel.FromAvailableSchoolDownloads(downloads);
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
                DownloadDataScope scope,
                ControllerContext context,
                IUrlHelper url,
                IAspApiClient api,
                Func<DownloadDataStepType, DownloadDataStep> getStep) 
                : base(path, title, scope, context, url, api, getStep)
            {
            }

            public override object GetRouteValues(DownloadDataStepParameters parameters)
                => InitialRouteValues;

            public override async Task<Result<IActionResult>> Handle(
                DownloadDataStepParameters parameters,
                string urnOrLACode,
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
                    from availableDownloads in await GetAvailableDownloads(urnOrLACode, Optional<int>.None)
                    let model = new DownloadDataStepViewModel(
                        Title,
                        new BreadcrumbTrailViewModel(
                            baseBreadcrumbTrail,
                            Title
                        ),
                        new DownloadDataSelectYearViewModel(availableDownloads.AvailableDates)
                    )
                    select showPageView(model);
            }
        }

        private class SelectFilesStep : DownloadDataStep
        {
            public SelectFilesStep(
                string path,
                string title,
                DownloadDataScope scope,
                ControllerContext context,
                IUrlHelper url,
                IAspApiClient api,
                Func<DownloadDataStepType, DownloadDataStep> getStep) 
                : base(path, title, scope, context, url, api, getStep)
            {
            }

            public override object GetRouteValues(DownloadDataStepParameters parameters)
                => new { step = Path.TrimEnd('/'), selectedYear = parameters.SelectedYear, selectedFiles = "" };

            public override async Task<Result<IActionResult>> Handle(
                DownloadDataStepParameters parameters,
                string urnOrLACode,
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
                    from availableDownloads in await GetAvailableDownloads(urnOrLACode, Optional.FromNullable(parameters.SelectedYear))
                    let model = new DownloadDataStepViewModel(
                        Title,
                        new BreadcrumbTrailViewModel(
                            baseBreadcrumbTrail.Concat([
                                new(selectYearStep.Title, Action(selectYearStep.GetRouteValues(parameters))),
                            ]),
                            Title
                        ),
                        new DownloadDataSelectFilesViewModel(availableDownloads.Downloads)
                    )
                    select showPageView(model);
            }
        }

        private class SelectFormatStep : DownloadDataStep
        {
            public SelectFormatStep(
                string path,
                string title,
                DownloadDataScope scope,
                ControllerContext context,
                IUrlHelper url,
                IAspApiClient api,
                Func<DownloadDataStepType, DownloadDataStep> getStep) 
                : base(path, title, scope, context, url, api, getStep)
            {
            }

            public override object GetRouteValues(DownloadDataStepParameters parameters)
                => new { step = Path.TrimEnd('/'), selectedYear = parameters.SelectedYear, selectedFiles = parameters.SelectedFiles };

            public override Task<Result<IActionResult>> Handle(
                DownloadDataStepParameters parameters,
                string urnOrLACode,
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
                        Scope == DownloadDataScope.LocalAuthority ? "LA" : "school",
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
                DownloadDataScope scope,
                ControllerContext context,
                IUrlHelper url,
                IAspApiClient api,
                Func<DownloadDataStepType, DownloadDataStep> getStep) 
                : base(path, title, scope, context, url, api, getStep)
            {
            }

            public override object GetRouteValues(DownloadDataStepParameters parameters)
                => new { step = Path.TrimEnd('/'), fileType = parameters.FileType, selectedFiles = parameters.SelectedFiles };

            public override async Task<Result<IActionResult>> Handle(
                DownloadDataStepParameters parameters,
                string urnOrLACode,
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