using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.DTO.Mapper;
using ASP.Core;
using ASP.Core.Establishments.Search;
using ASP.Core.Results;
using ASP.Core.Establishments;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Scoping;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch;

public class EstablishmentSearch : IEstablishmentSearch
{
    private readonly ILocalAuthorityRepository _localAuthorityRepository;
    private readonly IMultiAcademyTrustRepository _multiAcademyTrustRepository;
    private readonly EstablishmentSearchService _searchService;

    public EstablishmentSearch(
        IEstablishmentRepository establishmentRepository,
        ILocalAuthorityRepository localAuthorityRepository,
        IMultiAcademyTrustRepository multiAcademyTrustRepository
    )
    {
        _localAuthorityRepository = localAuthorityRepository;
        _multiAcademyTrustRepository = multiAcademyTrustRepository;

        _searchService = new EstablishmentSearchService(establishmentRepository);
    }

    public async Task<Result<SearchResultsPage<EstablishmentListingDTO>>> HandleRequest(
        EstablishmentSearchRequest request)
    {
        var page = request.Page.GetValueOrDefault(1);
        var resultsPerPage = request.ResultsPerPage.GetValueOrDefault(Constants.SearchResultPageSize);
        var scopeIdentifier = request.ScopeIdentifier.GetValueOrDefault("");

        return await Scope.Validate(request.ScopeType, scopeIdentifier, _localAuthorityRepository, _multiAcademyTrustRepository)
            .Then(scope => _searchService.Search(request.SearchTerm, scope, page, resultsPerPage))
            .Map(results => results.Map(r => r.MapToEstablishmentListingDTO()));
    }
}