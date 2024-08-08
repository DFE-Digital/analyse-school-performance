using ASP.Core.Establishments.Search;
using ASP.Core.Extensions;
using ASP.Core.Results;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.DTO.Mapper;
using ASP.Application.UseCases.LocalAuthorities.GetLocalAuthority;
using ASP.Application.UseCases.MultiAcademyTrusts.GetMultiAcademyTrust;
using ASP.Core.Establishments;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch;

public class EstablishmentSearch : IEstablishmentSearch
{
    private readonly IEstablishmentSearchStrategyFactory _establishmentSearchStrategyFactory;
    private readonly IAspApiClient _api;

    public EstablishmentSearch(IEstablishmentSearchStrategyFactory establishmentSearchStrategyFactory,
        IAspApiClient api)
    {
        _api = api;
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

        var validateScopeError = await ValidateScope(request);

        if (validateScopeError != null)
        {
            return validateScopeError;
        }

        var initialStrategy = _establishmentSearchStrategyFactory.CreateStrategy(
            request.Scope,
            searchType,
            request.SearchTerm,
            page,
            resultsPerPage
        );

        // Backup search strategy if the initial strategy fails (e.g. if it's a 3-digit code we'll do an LA
        // lookup but if we don't find a matching LA then we need to do a full search on name/address)
        var backupStrategy = _establishmentSearchStrategyFactory.CreateStrategy(
            request.Scope,
            SearchType.EstablishmentNameOrLocation,
            request.SearchTerm,
            page,
            resultsPerPage
        );

        var result = await initialStrategy.Execute()
            .IfErrorThen(
                e => e is NotFoundError && searchType != SearchType.EstablishmentNameOrLocation,
                backupStrategy.Execute
            )
            .Map(results => results.Map(r => r.MapToSearchResultDTO()));

        return result;
    }

    private async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResultDTO>>?> ValidateScope(
        EstablishmentSearchRequest request)
    {
        if (request.Scope.ScopeType == ScopeType.LA)
        {
            var laScopeInvalidError =
                Error.Invalid($@"Local Authority with code ""{request.Scope.ScopeIdentifier}"" does not exist.");

            // Validate Scope Identifier
            var localAuthority = await _api.GetLocalAuthority(
                new GetLocalAuthorityRequest(request.Scope.ScopeIdentifier)).MapError(e => e is NotFoundError
                ? laScopeInvalidError
                : e).GetValueOrDefault(new ASP.Application.UseCases.LocalAuthorities.DTO.LocalAuthorityDTO("", ""));

            if (string.IsNullOrEmpty(localAuthority.Code))
            {
                return laScopeInvalidError;
            }
        }

        if (request.Scope.ScopeType == ScopeType.MAT)
        {
            var matScopeInvalidError =
                Error.Invalid($@"Multi-Academy Trust with UID ""{request.Scope.ScopeIdentifier}"" does not exist.");

            // Validate Scope Identifier
            var mat = await _api.GetMultiAcademyTrust(
                new GetMultiAcademyTrustRequest(request.Scope.ScopeIdentifier)).MapError(e => e is NotFoundError
                ? matScopeInvalidError
                : e).GetValueOrDefault(
                new ASP.Application.UseCases.MultiAcademyTrusts.DTO.MultiAcademyTrustDTO("", ""));

            if (string.IsNullOrEmpty(mat.Id))
            {
                return matScopeInvalidError;
            }
        }

        return null;
    }
}