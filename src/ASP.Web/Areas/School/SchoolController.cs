using ASP.Application;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Core.Templating;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    public abstract class SchoolController : Controller
    {
        const string LANDING_PAGE_CONTENT_TEMPLATE_ID = "school-landing-page";
        const string USEFUL_LINKS_CONTENT_TEMPLATE_ID = "school-useful-links";
        const string OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID = "school-other-reports-ofsted";

        protected readonly IAspApiClient _api;
        protected readonly IHostEnvironment _hostEnvironment;

        protected SchoolController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        )
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        protected async Task<Result<SchoolLandingPageViewModel>> LandingPage(string urn, string? revision)
        {
            return await GetEstablishmentDetails(urn)
                .Then(establishmentDetails => GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                .Then(contentTemplate => GetSchoolPage(establishmentDetails)
                .Map(schoolPage => new SchoolLandingPageViewModel(
                    schoolPage,
                    establishmentDetails,
                    contentTemplate
                ))));
        }

        protected async Task<Result<SchoolContentPageViewModel>> OtherReports(string urn, string? revision)
        {
            return await GetEstablishmentDetails(urn)
                .Then(establishmentDetails => GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)
                .Then(contentTemplate => GetSchoolPage(establishmentDetails, "Other reports")
                .Map(schoolPage => new SchoolContentPageViewModel(
                    schoolPage,
                    contentTemplate
                ))));
        }

        protected async Task<Result<SchoolContentPageViewModel>> UsefulLinks(string urn, string? revision)
        {
            return await GetEstablishmentDetails(urn)
                .Then(establishmentDetails => GetContentTemplate(USEFUL_LINKS_CONTENT_TEMPLATE_ID, revision)
                .Then(contentTemplate => GetSchoolPage(establishmentDetails, "Useful links")
                .Map(schoolPage => new SchoolContentPageViewModel(
                    schoolPage,
                    contentTemplate
                ))));
        }

        protected async Task<Result<SchoolPageViewModel>> DownloadData(string urn)
        {
            return await GetEstablishmentDetails(urn)
                .Then(establishmentDetails => GetSchoolPage(establishmentDetails, "Download data"));
        }

        protected abstract Task<Result<SchoolPageViewModel>> GetSchoolPage(EstablishmentDetailsViewModel establishmentDetails, string? page = null);

        protected virtual async Task<Result<EstablishmentDetailsViewModel>> GetEstablishmentDetails(string urn)
        {
            return await _api.GetEstablishmentDetails(new GetEstablishmentDetailsRequest(urn))
                .Map(EstablishmentDetailsViewModel.FromEstablishmentDetails);
        }

        protected virtual async Task<Result<ContentTemplateViewModel>> GetContentTemplate(string contentId, string? revision)
        {
            return await _api.ViewContentTemplate(new ViewContentTemplateRequest(contentId, Optional.FromNullable(revision)))
                .Map(template => ContentTemplateViewModel.FromTemplate(contentId, revision, template))
                .DefaultIf(error => error is NotFoundError, new ContentTemplateViewModel());
        }
    }
}
