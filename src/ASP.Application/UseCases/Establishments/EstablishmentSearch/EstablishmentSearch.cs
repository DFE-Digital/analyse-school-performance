using ASP.Core.Extensions;
using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Core.Search.Strategy;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch;

public class EstablishmentSearch : IEstablishmentSearch
{
    private readonly IEstablishmentSearchStrategyFactory _establishmentSearchStrategyFactory;
    
    public EstablishmentSearch(IEstablishmentSearchStrategyFactory establishmentSearchStrategyFactory)
    {
        _establishmentSearchStrategyFactory = establishmentSearchStrategyFactory;
    }

    public async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResultDTO>>> HandleRequest(EstablishmentSearchRequest request)
    {
        var searchType = request.SearchTerm.ClassifySearchType();
        var page = request.Page ?? 1;
        var resultsPerPage = request.ResultsPerPage ?? 50;

        if (searchType == SearchType.Invalid)
        {
            return Error.Invalid($@"Bad request: the parameter ""{nameof(request.SearchTerm)}"" : ""{request.SearchTerm}"" with type ""{searchType}"" is invalid");
        }

        var initialStrategy = _establishmentSearchStrategyFactory.CreateStrategy(
            searchType, 
            request.SearchTerm, 
            page, 
            resultsPerPage
        );

        // Backup search strategy if the initial strategy fails (e.g. if it's a 3-digit code we'll do an LA
        // lookup but if we don't find a matching LA then we need to do a full search on name/address)
        var backupStrategy = _establishmentSearchStrategyFactory.CreateStrategy(
            SearchType.EstablishmentNameOrLocation,
            request.SearchTerm,
            page,
            resultsPerPage
        );

        return await initialStrategy.Execute()
            .IfErrorThen(
                e => e is NotFoundError && searchType != SearchType.EstablishmentNameOrLocation,
                backupStrategy.Execute
            );
    }
}