using ASP.Application;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Application.UseCases.Downloads.DownloadAsZip;
using ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Utilities;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Core.Templating;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    public abstract class BaseSchoolController : Controller
    {
        protected const string LANDING_PAGE_CONTENT_TEMPLATE_ID = "school-landing-page";
        protected const string USEFUL_LINKS_CONTENT_TEMPLATE_ID = "school-useful-links";
        protected const string OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID = "school-other-reports-ofsted";

        protected readonly IAspApiClient _api;
        protected readonly IHostEnvironment _hostEnvironment;

        protected BaseSchoolController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        )
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        protected virtual Task<Result<EstablishmentDetailsViewModel>> GetEstablishmentDetails(string urn)
        {
            return 
                from establishmentDetails in _api.GetEstablishmentDetails(new GetEstablishmentDetailsRequest(urn))
                select EstablishmentDetailsViewModel.FromEstablishmentDetails(establishmentDetails);
        }

        protected virtual Task<Result<ContentTemplateViewModel>> GetContentTemplate(string contentId, string? revision)
        {
            return (
                from template in _api.ViewContentTemplate(new ViewContentTemplateRequest(contentId, Optional.FromNullable(revision)))
                select ContentTemplateViewModel.FromTemplate(contentId, revision, template)
            ).DefaultIf(error => error is NotFoundError, new ContentTemplateViewModel());
        }

        protected virtual Task<Result<AvailableDownloadsViewModel>> GetAvailableDownloads(string urn, Optional<int> year)
        {
            return 
                from downloads in _api.GetAvailableSchoolDownloads(new GetAvailableSchoolDownloadsRequest(urn, year))
                select AvailableDownloadsViewModel.FromAvailableDownloads(downloads);
        }

        protected virtual Task<Result<ActionResult>> DownloadDataAsZip(FileType fileType, List<string> fileIds)
        {
            return 
                from response in _api.DownloadAsZipFile(new DownloadAsZipFileRequest(fileType, fileIds))
                select (ActionResult)new FileStreamResult(response.Content, response.ContentType) {
                    FileDownloadName = response.FileName
                };
        }
    }
}
