using ASP.Api.Client;
using ASP.Api.Client.Downloads;
using ASP.Core.Results;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.SubController;
using Microsoft.AspNetCore.Mvc;

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
        : SubController<DownloadDataController, DownloadDataParameters, string, DownloadDataSubActionType, DownloadDataSubActionViewModel>
    {
        private readonly DownloadsScopeType _scopeType;
        private readonly IAspApiClient _api;

        /// <summary>
        /// Creates a <c>DownloadDataController</c> instance to handle data downloads.
        /// </summary>
        /// <param name="hostAction">Action that will "host" the search sub-actions</param>
        /// <param name="hostController">Controller the "host" action belongs to</param>
        /// <param name="hostActionRouteValueKeys">Keys of any route values the host action requires</param>
        /// <param name="scopeType">Whether the downloads are School or LA downloads</param>
        /// <param name="api">API client instance</param>
        /// <param name="stepRouteConfig">Optional config to override the default paths for each step</param>
        public DownloadDataController(
            string hostAction,
            string hostController,
            List<string> hostActionRouteValueKeys,
            DownloadsScopeType scopeType,
            IAspApiClient api,
            Dictionary<DownloadDataSubActionType, SubActionRouteConfig>? stepRouteConfig = null)
            : base(
                hostAction,
                hostController,
                hostActionRouteValueKeys,
                stepRouteConfig)
        {
            _scopeType = scopeType;
            _api = api;

            AddSubAction(DownloadDataSubActionType.SelectYear,
                new SelectYearStep(this, "", "Dates available for download"));

            AddSubAction(DownloadDataSubActionType.SelectFiles,
                new SelectFilesStep(this, "select-files", "Data files available for download"));

            AddSubAction(DownloadDataSubActionType.SelectFormat,
                new SelectFormatStep(this, "select-format", "Download data"));

            AddSubAction(DownloadDataSubActionType.DownloadAsZip,
                new DownloadAsZipStep(this, "download-as-zip", "Download as Zip file"));
        }

        private class SelectYearStep : SubAction
        {
            public SelectYearStep(
                DownloadDataController controller,
                string path,
                string title,
                string subtitle = "")
                : base(controller, path, title, subtitle)
            {
            }

            public override RouteValueDictionary GetRouteValues(DownloadDataParameters parameters)
                => base.GetRouteValues(parameters).Merge(new { selectedYear = "", selectedFiles = "" });

            public override async Task<Result<IActionResult>> Handle(
                DownloadDataParameters parameters,
                string scopeIdentifier,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<DownloadDataSubActionViewModel, IActionResult> showPageView)
            {
                if (Controller.RequestMethod == HttpMethods.Post)
                {
                    // Redirect to files selection if year is valid
                    if (parameters.SelectedYear.HasValue)
                    {
                        return Controller.RedirectToSubAction(DownloadDataSubActionType.SelectFiles, parameters);
                    }

                    Controller.AddModelError(Constants.ModelErrorKeySelectedYear,
                        Constants.AcademicYearToDownldValidationErrorMessage);
                }

                return
                    from availableDownloads in await Controller.DownloadsGetAll(scopeIdentifier, null)
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
                    let model = new DownloadDataSubActionViewModel(
                        title,
                        new BreadcrumbTrailViewModel(
                            baseBreadcrumbTrail
                        ),
                        downloadDataModel
                    )
                    select showPageView(model);
            }
        }

        private class SelectFilesStep : SubAction
        {
            public SelectFilesStep(
                DownloadDataController controller,
                string path,
                string title,
                string subtitle = "")
                : base(controller, path, title, subtitle)
            {
            }

            public override RouteValueDictionary GetRouteValues(DownloadDataParameters parameters)
                => base.GetRouteValues(parameters).Merge(new { selectedYear = parameters.SelectedYear, selectedFiles = "" });

            public override async Task<Result<IActionResult>> Handle(
                DownloadDataParameters parameters,
                string scopeIdentifier,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<DownloadDataSubActionViewModel, IActionResult> showPageView)
            {
                if (Controller.RequestMethod == HttpMethods.Post)
                {
                    var selectedFiles = parameters.SelectedFiles ?? new();

                    // Redirect to data select format page if year is valid
                    if (selectedFiles.Any())
                    {
                        return Controller.RedirectToSubAction(DownloadDataSubActionType.SelectFormat, parameters);
                    }

                    Controller.AddModelError(Constants.ModelErrorKeySelectedFiles,
                        Constants.DataFilesAvialableForDownlodValidationErrorMessage);
                }

                return
                    from availableDownloads in await Controller.DownloadsGetAll(scopeIdentifier, parameters.SelectedYear)
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
                            Controller.GetUrlForSubAction(DownloadDataSubActionType.SelectYear, parameters))
                    let model = new DownloadDataSubActionViewModel(
                        title,
                        new BreadcrumbTrailViewModel(
                            baseBreadcrumbTrail.Concat([
                                Controller.GetBreadcrumbForSubAction(DownloadDataSubActionType.SelectYear, parameters)
                            ])
                        ),
                        downloadDataModel
                    )
                    select showPageView(model);
            }
        }

        private class SelectFormatStep : SubAction
        {
            public SelectFormatStep(
                DownloadDataController controller,
                string path,
                string title,
                string subtitle = "")
                : base(controller, path, title, subtitle)
            {
            }

            public override RouteValueDictionary GetRouteValues(DownloadDataParameters parameters)
                => base.GetRouteValues(parameters).Merge(new { selectedYear = parameters.SelectedYear, selectedFiles = parameters.SelectedFiles });

            public override Task<Result<IActionResult>> Handle(
                DownloadDataParameters parameters,
                string scopeIdentifier,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<DownloadDataSubActionViewModel, IActionResult> showPageView)
            {

                List<(string, string?)> fileFormatList = [];

                foreach (FileType fileType in Enum.GetValues<FileType>())
                {
                    fileFormatList.Add(($"Data in {fileType} format", Controller.GetUrlForSubAction(DownloadDataSubActionType.DownloadAsZip, parameters with { FileType = fileType })));
                }

                var model = new DownloadDataSubActionViewModel(
                    Title,
                    new BreadcrumbTrailViewModel(
                        baseBreadcrumbTrail.Concat([
                            Controller.GetBreadcrumbForSubAction(DownloadDataSubActionType.SelectYear, parameters),
                            Controller.GetBreadcrumbForSubAction(DownloadDataSubActionType.SelectFiles, parameters)
                        ])
                    ),
                    new DownloadDataSelectFormatViewModel(
                        Controller.ScopeTypeLabel,
                        fileFormatList,
                        Controller.GetUrlForSubAction(DownloadDataSubActionType.SelectYear, parameters)
                    )
                );

                return Task.FromResult(Result.Success(showPageView(model)));
            }
        }

        private class DownloadAsZipStep : SubAction
        {
            public DownloadAsZipStep(
                DownloadDataController controller,
                string path,
                string title,
                string subtitle = "")
                : base(controller, path, title, subtitle)
            {
            }

            public override RouteValueDictionary GetRouteValues(DownloadDataParameters parameters)
                => base.GetRouteValues(parameters).Merge(new { fileType = parameters.FileType, selectedFiles = parameters.SelectedFiles });

            public override async Task<Result<IActionResult>> Handle(
                DownloadDataParameters parameters,
                string scopeIdentifier,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<DownloadDataSubActionViewModel, IActionResult> showPageView)
            {
                var fileType = parameters.FileType ?? FileType.CSV;
                var selectedFiles = parameters.SelectedFiles ?? new();

                return
                    from response in await Controller._api.DownloadsGetPackage(new DownloadsGetPackageRequest(fileType, selectedFiles, Controller._scopeType, scopeIdentifier))
                    select (IActionResult)new FileStreamResult(response.Content, response.ContentType)
                    {
                        FileDownloadName = response.FileName
                    };
            }
        }

        private string ScopeTypeLabel
            => _scopeType == DownloadsScopeType.LA ? "LA" : "school";

        private Task<Result<AvailableDownloadsViewModel>> DownloadsGetAll(string scopeIdentifier, int? year)
        {
            return
                from downloads in _api.DownloadsGetAll(new DownloadsGetAllRequest(_scopeType, scopeIdentifier, year))
                select AvailableDownloadsViewModel.FromAvailableDownloads(downloads);
        }
    }
}