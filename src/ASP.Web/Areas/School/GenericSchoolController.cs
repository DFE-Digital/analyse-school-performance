using ASP.Application;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Core.Authorization;
using ASP.Core.Results;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Areas.Shared.DownloadData;
using ASP.Web.Areas.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("school/{urn}")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToGenericSchool)]
    public class GenericSchoolController : SchoolController
    {
        public GenericSchoolController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        ) : base(api, hostEnvironment)
        {
        }

        [HttpGet("")]
        public new Task<IActionResult> LandingPage(string urn, string? revision)
        {
            return base.LandingPage(urn, revision)
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("other-reports")]
        public Task<IActionResult> OtherReports(string urn, string? revision)
        {
            return base.OtherReports(urn, revision, schoolName => new([], "Other reports"))
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("useful-links")]
        public Task<IActionResult> UsefulLinks(string urn, string? revision)
        {
            return base.UsefulLinks(urn, revision, schoolName => new([], "Useful links"))
                .ToActionResult(View, _hostEnvironment);
        }

        protected override BreadcrumbTrailViewModel GetLandingPageBreadcrumbs(string schoolName,
            LocalAuthorityDTO? localAuthority)
        {
            var breadcrumbs = new List<BreadcrumbItem>();

            if (User.Role()!.HasAccessToAllSchools)
            {
                var laCode = localAuthority?.Code ?? "";
                var laName = localAuthority?.Name ?? "";
                breadcrumbs =
                [
                    new("All local authorities", $"/local-authorities"),
                    new(laName, $"/local-authority/{laCode}"),
                    new("All schools", $"/local-authority/{laCode}/schools")
                ];
            }
            else if (User.Role()!.HasAccessToMySchools)
            {
                breadcrumbs =
                [
                    new("My schools", $"/my-schools")
                ];
            }

            return new BreadcrumbTrailViewModel(breadcrumbs, schoolName);
        }

        protected override IEnumerable<BreadcrumbItem> GetChildPageBreadcrumbs(string urn, string schoolName) => [
            new("My schools", $"/my-schools/"),
            new(schoolName, $"/school/{urn}"),
        ];

        protected override NavigationViewModel GetSubNavigation(EstablishmentDetailsViewModel establishmentDetails, PathString requestPath)
        {
            return new NavigationViewModel(new([
                new("download-data", "Download data", $"/school/{establishmentDetails.Urn}/download-data/", requestPath),
                new("other-reports", "Other reports", $"/school/{establishmentDetails.Urn}/other-reports/", requestPath),
                new("useful-links", "Useful links", $"/school/{establishmentDetails.Urn}/useful-links/", requestPath)
            ]));
        }

        protected override NavigationViewModel GetSideNavigation(EstablishmentDetailsViewModel establishmentDetails, PathString requestPath)
        {
            return new NavigationViewModel(new([
                new("name", $"{establishmentDetails.Name} data", $"/school/{establishmentDetails.Urn}/download-data/", requestPath)
            ]));
        }

        protected override Task<Result<SchoolPageViewModel>> GetSchoolPage(
            EstablishmentDetailsViewModel establishmentDetails,
            BreadcrumbTrailViewModel breadcrumbs,
            NavigationViewModel? subNavigation,
            NavigationViewModel? sideNavigation)
        {
            string title = string.Empty, subtitle = string.Empty;

            if (User.Role()!.HasAccessToAllSchools)
            {
                 title = $"{establishmentDetails.Name}";
                 subtitle = $"<span>(URN: {establishmentDetails.Urn})</span>";
                 
            } else if (User.Role()!.HasAccessToMySchools)
            {
                title = "My schools";
                subtitle = $"{establishmentDetails.Name} <span>(URN: {establishmentDetails.Urn})</span>";
            }
            
            var schoolPage = new SchoolPageViewModel(
                "GenericSchool",
                title,
                subtitle,
                establishmentDetails.Name,
                establishmentDetails.Urn,
                breadcrumbs,
                subNavigation,
                sideNavigation
            );

            return Task.FromResult(Result.Success(schoolPage));
        }
        
        protected override Task<Result<BaseDownloadDataModel>> GetDownloadDataPage(
            EstablishmentDetailsViewModel establishmentDetails,
            BreadcrumbTrailViewModel breadcrumb,
            string currentActionName,
            NavigationViewModel? subNavigation = null,
            NavigationViewModel? sideNavigation = null)
        {
            var title = "Download data";
            var subtitle = $@"{establishmentDetails.Name} <span class=""govuk-!-font-weight-regular"">(URN: {establishmentDetails.Urn})</span>";
            
            var (contentTitle, contentTitleCaption) = currentActionName switch
            {
                nameof(DownloadDataSelectYear) => (
                    "Dates available for download",
                    $"{establishmentDetails.Name} data"
                ),
                nameof(DownloadDataSelectFiles) => (
                    "Data files available for download",
                    $"{establishmentDetails.Name} data"
                ),
                nameof(DownloadDataSelectFormat) => (
                    $"Download {establishmentDetails.Name} data",
                    $"{establishmentDetails.Name} data"
                ),
                _ => ("", "") // Default case
            };
            
            var downloadDataPage = new BaseDownloadDataModel(
                breadcrumb,
                subNavigation,
                sideNavigation,
                title,
                subtitle,
                contentTitle,
                contentTitleCaption,
                "GenericSchool",
                ""
            );
            return Task.FromResult(Result.Success(downloadDataPage));
        }
    }
}
