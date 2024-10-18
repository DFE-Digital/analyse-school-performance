using ASP.Application;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.GetAllEstablishments;
using ASP.Core;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Scoping;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Areas.Shared.EstablishmentListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School;

public abstract class SchoolsController : Controller
{
    protected readonly IAspApiClient _api;
    protected readonly IHostEnvironment _hostEnvironment;

    protected SchoolsController(
        IAspApiClient api,
        IHostEnvironment hostEnvironment
    )
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    }

    protected Task<Result<ScopedResultsPage<EstablishmentListingDTO>>> GetAllEstablishments(
        ScopeType scopeType, Optional<string> scopeId, int pageNumber)
    {
        var request = new GetAllEstablishmentsRequest(
            scopeType,
            scopeId,
            Optional<int>.Some(pageNumber),
            Optional<int>.Some(Constants.SearchResultPageSize)
        );

        return _api.GetAllEstablishments(request)
            .DefaultIf(error => error is NotFoundError,
                new ScopedResultsPage<EstablishmentListingDTO>());
    }

    protected SchoolsPageViewModel DefaultViewModel(ScopedResultsPage<EstablishmentListingDTO> result,
        string title, string subTitle, string paginationUrl)
    {
        var breadcrumbTrail = new BreadcrumbTrailViewModel(title);

        return new SchoolsPageViewModel(
            title,
            subTitle,
            result.TotalResults,
            new PaginationModel(
                paginationUrl ?? "",
                result.Page,
                result.TotalResults,
                result.ResultsPerPage,
                "school",
                "schools"
            ),
            breadcrumbTrail,
            EstablishmentListingModel.FromEstablishmentListingDto(result.Results)
        );
    }
}