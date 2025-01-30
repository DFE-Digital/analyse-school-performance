using ASP.Application;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Domain.Establishments.UseCases.GetEstablishmentDetails;
using ASP.Domain.Templating.UseCases.ViewContentTemplate;
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

        protected string Action(string action, object? values = null) 
            => Url.Action(action, values) 
                ?? "";
    }
}
