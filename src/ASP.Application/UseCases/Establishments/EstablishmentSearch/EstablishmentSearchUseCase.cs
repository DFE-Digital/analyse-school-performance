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
            return Error.Validation($@"Bad request: the parameter ""{nameof(request.SearchTerm)}"" : ""{request.SearchTerm}"" with type ""{searchType}"" is invalid");
        }
        
        var inputSearchTerm = request.SearchTerm;
        
        if (searchType == SearchType.LocalAuthEstablishment7Digit)
        {
            inputSearchTerm = inputSearchTerm.ToLaEstabCodeFormat();
        }

        var searchStrategy = _establishmentSearchStrategyFactory.CreateStrategy(inputSearchTerm, request.Page);

        var results = await searchStrategy.Execute();

        return results;
    }
}