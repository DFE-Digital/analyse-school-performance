using ASP.Core.Extensions;
using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Core.Search.Strategy;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch;

public class EstablishmentSearchUseCase : IEstablishmentSearchUseCase
{
    private readonly IEstablishmentSearchStrategyFactory _establishmentSearchStrategyFactory;
    
    public EstablishmentSearchUseCase(IEstablishmentSearchStrategyFactory establishmentSearchStrategyFactory)
    {
        _establishmentSearchStrategyFactory = establishmentSearchStrategyFactory;
    }

    public async Task<Result<SearchResult<EstablishmentDetailsSearchResultDTO>>> HandleRequest(EstablishmentSearchUseCaseRequest request)
    {
        var searchType = request.SearchTerm.ClassifySearchType();
        
        if (searchType == SearchType.Invalid)
        {
            return Error.Invalid($@"Bad request: the parameter ""{nameof(request.SearchTerm)}"" : ""{request.SearchTerm}"" with type ""{searchType}"" is invalid");
        }

        var searchStrategy = _establishmentSearchStrategyFactory.CreateStrategy(searchType, request.SearchTerm, request.Page, request.ResultsPerPage);

        var searchResults = await searchStrategy.Execute();
        
        var results = searchResults.GetValueOrDefault(new SearchResult<EstablishmentDetailsSearchResultDTO>());

        if (results.TotalResults == 0 && (searchType != SearchType.EstablishmentNameOrLocation && searchType != SearchType.Urn))

        {
            searchType = SearchType.EstablishmentNameOrLocation;
            searchStrategy = _establishmentSearchStrategyFactory.CreateStrategy(searchType, request.SearchTerm,
                request.Page, request.ResultsPerPage);
            searchResults = await searchStrategy.Execute();
        }

        return searchResults;
    }
}