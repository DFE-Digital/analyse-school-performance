using ASP.Api.Client;
using ASP.Api.Client.Schools;
using ASP.Api.Client.ContentTemplates;
using ASP.Core.Results;
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

        protected virtual Task<Result<SchoolDetailsViewModel>> GetSchoolDetails(string urn)
        {
            return 
                from establishmentDetails in _api.SchoolsGetSingle(new SchoolsGetSingleRequest(urn))
                select SchoolDetailsViewModel.FromSchoolDetails(establishmentDetails);
        }

        protected virtual Task<Result<ContentTemplateViewModel>> GetContentTemplate(string contentId, string? revision)
        {
            return (
                from template in _api.ContentTemplatesGetSingle(new ContentTemplatesGetSingleRequest(contentId, revision))
                select ContentTemplateViewModel.FromTemplate(contentId, revision, template)
            ).DefaultIf(error => error is NotFoundError, new ContentTemplateViewModel());
        }
        
        protected virtual Task<Result<LinkedSchoolsViewModel>> GetLinkedSchools(string urn, Func<string, string?> createSchoolUrl)
        {
            return (
                from linkedEstablishment in _api.SchoolsGetLinkedSchools(new SchoolsGetLinkedSchoolsRequest(urn))
                select new LinkedSchoolsViewModel(urn, linkedEstablishment.Links, createSchoolUrl)
            );
        }

        protected string Action(string action, object? values = null) 
            => Url.Action(action, values) 
                ?? "";
    }
}
