using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.DTO.Mapper;
using ASP.Core.Establishments.Search;
using ASP.Core.Extensions;
using ASP.Core.Results;
using ASP.Core.Establishments;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch;

public class EstablishmentSearch : IEstablishmentSearch
{
    private readonly IEstablishmentSearchStrategyFactory _establishmentSearchStrategyFactory;
    private readonly ILocalAuthorityRepository _localAuthorityRepository;
    private readonly IMultiAcademyTrustRepository _multiAcademyTrustRepository;

    public EstablishmentSearch(IEstablishmentSearchStrategyFactory establishmentSearchStrategyFactory,
        ILocalAuthorityRepository localAuthorityRepository,
        IMultiAcademyTrustRepository multiAcademyTrustRepository)
    {
        _localAuthorityRepository = localAuthorityRepository;
        _multiAcademyTrustRepository = multiAcademyTrustRepository;
        _establishmentSearchStrategyFactory = establishmentSearchStrategyFactory;
    }

    public async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResultDTO>>> HandleRequest(
        EstablishmentSearchRequest request)
    {
        var searchType = request.SearchTerm.ClassifySearchType();
        var page = request.Page ?? 1;
        var resultsPerPage = request.ResultsPerPage ?? 50;

        if (searchType == SearchType.Invalid)
        {
            return Error.Invalid(
                $@"The parameter ""{nameof(request.SearchTerm)}"" : ""{request.SearchTerm}"" with type ""{searchType}"" is invalid");
        }

        var scope = new Scope(request.ScopeType, request.ScopeIdentifier);

        var initialStrategy = _establishmentSearchStrategyFactory.CreateStrategy(
            scope,
            searchType,
            request.SearchTerm,
            page,
            resultsPerPage
        );

        // Backup search strategy if the initial strategy fails (e.g. if it's a 3-digit code we'll do an LA
        // lookup but if we don't find a matching LA then we need to do a full search on name/address)
        var backupStrategy = _establishmentSearchStrategyFactory.CreateStrategy(
            scope,
            SearchType.EstablishmentNameOrLocation,
            request.SearchTerm,
            page,
            resultsPerPage
        );

        return await scope
            .Validate(_localAuthorityRepository, _multiAcademyTrustRepository)
            .Then(async x => await initialStrategy.Execute())
            .IfErrorThen(
                e => e is NotFoundError && searchType != SearchType.EstablishmentNameOrLocation,
                backupStrategy.Execute
            ).Map(results => results.Map(r => r.MapToSearchResultDTO()));
    }
}