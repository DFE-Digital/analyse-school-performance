using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.Search;

[Area("Search")]
[Route("search")]
[ServiceFilter<TermsOfUseActionFilter>]
public class SearchController : Controller
{
    private readonly IAspApi _api;
    private readonly IHostEnvironment _hostEnvironment;

    public SearchController(
        IAspApi api,
        IHostEnvironment hostEnvironment
    )
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("search-result")]
    public async Task<IActionResult> SearchResult(SearchParams searchParams)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var estabSearchRequest = new EstablishmentSearchUseCaseRequest(
                searchParams.SearchTerm,
                searchParams.Page
            );

            var response = await _api.EstablishmentSearch(estabSearchRequest);

            var searchResult = response.GetValueOrDefault(new SearchResult<EstablishmentDetailsSearchResultDTO>());

            if (searchResult.TotalCount == 0)
            {
                return ReturnNoResultsResponse(estabSearchRequest, response);
            }

            if (searchResult.TotalCount == 1)
            {
                return RedirectToSchoolDetail(searchResult);
            }

            return ReturnDefaultSearchResponse(searchParams, estabSearchRequest, response);
        } catch(Exception ex)
        {
            return new ObjectResult(ex.Message) { StatusCode = 500 };
        }
    }

    private IActionResult ReturnNoResultsResponse(EstablishmentSearchUseCaseRequest request, 
        Result<SearchResult<EstablishmentDetailsSearchResultDTO>> result)
    {
        return result.Map(x =>
            new SearchViewModel
            {
                SearchTerm = request.SearchTerm,
                TotalCount = x.TotalCount
            }).ToActionResult(View, _hostEnvironment);
    }

    private IActionResult RedirectToSchoolDetail(SearchResult<EstablishmentDetailsSearchResultDTO> searchResult)
    {
        var result = searchResult.Results.FirstOrDefault();
        return RedirectToAction("Index", "School", new { area = "School", urn = result!.Urn });
    }

    private IActionResult ReturnDefaultSearchResponse(SearchParams searchParams,
        EstablishmentSearchUseCaseRequest request, Result<SearchResult<EstablishmentDetailsSearchResultDTO>> result)
    {
        return result.Map(x => new SearchViewModel
            {
                SearchResults =
                    EstablishmentSearchResultsModel.FromEstablishmentDetails(
                        x.Results),
                PaginationModel = new PaginationModel
                {
                    TotalCount = x.TotalCount,
                    SearchTerm = request.SearchTerm,
                    CurrentPage = searchParams.Page,
                    Skip = x.Skip,
                    ResultCount = x.ResultCount
                },
                SearchTerm = request.SearchTerm,
                TotalCount = x.TotalCount
            }).ToActionResult(View, _hostEnvironment);
    }
}