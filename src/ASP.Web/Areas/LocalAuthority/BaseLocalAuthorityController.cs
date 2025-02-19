using ASP.Api.Client;
using ASP.Api.Client.Establishments;
using ASP.Core.Results;
using ASP.Web.Core.Templating;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.LocalAuthority
{
    public abstract class BaseLocalAuthorityController : Controller
    {
        protected const string LANDING_PAGE_CONTENT_TEMPLATE_ID = "la-landing-page";

        protected readonly IAspApiClient _api;
        protected readonly IHostEnvironment _hostEnvironment;

        protected BaseLocalAuthorityController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        protected virtual Task<Result<string>> GetLocalAuthorityName(string laCode)
        {
            return
                from la in _api.GetLocalAuthority(new(laCode))
                select string.IsNullOrWhiteSpace(la.Name)
                    ? "Missing local authority name"
                    : la.Name;
        }

        protected virtual Task<Result<string>> GetEstablishmentName(string urn)
        {
            return
                from school in _api.GetEstablishmentDetails(new GetEstablishmentDetailsRequest(urn))
                select string.IsNullOrWhiteSpace(school.Name)
                    ? "Missing school name"
                    : school.Name;
        }

        protected virtual Task<Result<ContentTemplateViewModel>> GetContentTemplate(string contentTemplateId, string? revision)
        {
            var model =
                from template in _api.GetContentTemplate(new(contentTemplateId, revision))
                select ContentTemplateViewModel.FromTemplate(contentTemplateId, revision, template);

            return model
                .DefaultIf(error => error is NotFoundError, new ContentTemplateViewModel());
        }

        protected string Action(string action, object? values = null) 
            => Url.Action(action, values) 
                ?? "";
    }
}