using ASP.Application;
using ASP.Core.Optionality;
using ASP.Core.Results;
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

        protected async Task<Result<LocalAuthorityContentPageViewModel>> LandingPage(string laCode, string? revision)
        {
            return await GetLocalAuthorityName(laCode)
                .Then(laName => GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                .Then(contentTemplate => GetLocalAuthorityPage(laCode, laName)
                .Map(page => new LocalAuthorityContentPageViewModel(
                    page,
                    contentTemplate
                ))));
        }

        protected async Task<Result<LocalAuthorityPageViewModel>> DownloadData(string laCode)
        {
            return await GetLocalAuthorityName(laCode)
                .Then(laName => GetLocalAuthorityPage(laCode, laName));
        }

        protected abstract Task<Result<LocalAuthorityPageViewModel>> GetLocalAuthorityPage(string laCode, string laName, string? page = null);

        protected virtual Task<Result<string>> GetLocalAuthorityName(string laCode)
        {
            return _api.GetLocalAuthority(new(laCode))
                .Map(la => !string.IsNullOrWhiteSpace(la.Name)
                    ? la.Name
                    : "Missing local authority name");
        }

        protected virtual Task<Result<ContentTemplateViewModel>> GetContentTemplate(string contentTemplateId, string? revision)
        {
            return _api.ViewContentTemplate(new(contentTemplateId, Optional.FromNullable(revision)))
                .Map(template => ContentTemplateViewModel.FromTemplate(contentTemplateId, revision, template))
                .DefaultIf(error => error is NotFoundError, new ContentTemplateViewModel());
        }
    }
}