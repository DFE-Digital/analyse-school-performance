using ASP.Application;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Core.Templating;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.LocalAuthority
{
    public abstract class LocalAuthorityController : Controller
    {
        const string LANDING_PAGE_CONTENT_TEMPLATE_ID = "la-landing-page";

        protected readonly IAspApiClient _api;
        protected readonly IHostEnvironment _hostEnvironment;

        protected LocalAuthorityController(IAspApiClient api, IHostEnvironment hostEnvironment)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }


        protected Task<Result<LocalAuthorityContentPageViewModel>> LandingPage(string laCode, string? revision)
        {
            return
                from laName in GetLocalAuthorityName(laCode)
                from contentTemplate in GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                from page in GetLocalAuthorityPage(laCode, laName, GetLandingPageBreadcrumbs(laCode, laName))
                select new LocalAuthorityContentPageViewModel(
                    page,
                    contentTemplate
                );
        }


        protected Task<Result<LocalAuthorityPageViewModel>> DownloadData(string laCode, BreadcrumbTrailViewModel breadcrumbs)
        {
            return
                from laName in GetLocalAuthorityName(laCode)
                from page in GetLocalAuthorityPage(laCode, laName, 
                    breadcrumbs.Prepend(GetChildPageBreadcrumbs(laCode, laName)))
                select page;
        }

        protected abstract BreadcrumbTrailViewModel GetLandingPageBreadcrumbs(string laCode, string laName);
        
        protected abstract IEnumerable<BreadcrumbItem> GetChildPageBreadcrumbs(string laCode, string laName);
        
        protected abstract Task<Result<LocalAuthorityPageViewModel>> GetLocalAuthorityPage(string laCode, string laName, BreadcrumbTrailViewModel breadcrumbs);

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
    }
}