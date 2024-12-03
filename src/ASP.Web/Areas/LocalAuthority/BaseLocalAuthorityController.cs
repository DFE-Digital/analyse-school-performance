using ASP.Application;
using ASP.Application.UseCases.Downloads.DownloadAsZip;
using ASP.Application.UseCases.Downloads.GetAvailableLADownloads;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Utilities;
using ASP.Web.Core.Templating;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.LocalAuthority
{
    public abstract class BaseLocalAuthorityController : Controller
    {
        protected const string LANDING_PAGE_CONTENT_TEMPLATE_ID = "la-landing-page";

        protected readonly IAspApiClient _api;
        protected readonly IHostEnvironment _hostEnvironment;

        protected BaseLocalAuthorityController(IAspApiClient api, IHostEnvironment hostEnvironment)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        protected Task<Result<ActionResult>> DownloadDataAsZip(
            FileType fileType, 
            List<string> fileIds
        )
        {
            return
                from response in _api.DownloadAsZipFile(new DownloadAsZipFileRequest(fileType, fileIds))
                select (ActionResult) new FileStreamResult(response.Content, response.ContentType) {
                    FileDownloadName = response.FileName
                };
        }

        protected virtual Task<Result<string>> GetLocalAuthorityName(string laCode)
        {
            return
                from la in _api.GetLocalAuthority(new(laCode))
                select string.IsNullOrWhiteSpace(la.Name)
                    ? "Missing local authority name"
                    : la.Name;
        }

        protected virtual Task<Result<ContentTemplateViewModel>> GetContentTemplate(string contentTemplateId, string? revision)
        {
            var model =
                from template in _api.ViewContentTemplate(new(contentTemplateId, Optional.FromNullable(revision)))
                select ContentTemplateViewModel.FromTemplate(contentTemplateId, revision, template);

            return model
                .DefaultIf(error => error is NotFoundError, new ContentTemplateViewModel());
        }

        protected virtual Task<Result<AvailableDownloadsViewModel>> GetAvailableLaDownloads(string laCode, Optional<int> year)
        {
            return
                from downloads in _api.GetAvailableLaDownloads(new GetAvailableLADownloadsRequest(laCode, year))
                select AvailableDownloadsViewModel.FromAvailableDownloads(downloads);
        }
    }
}