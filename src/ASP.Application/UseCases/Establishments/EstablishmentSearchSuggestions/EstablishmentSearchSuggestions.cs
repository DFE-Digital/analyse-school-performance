using ASP.Core.Establishments;
using ASP.Core.Establishments.Search;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Extensions;
using ASP.Core.Results;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.DTO.Mapper;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;

public class EstablishmentSearchSuggestions : IEstablishmentSearchSuggestions
{
    private readonly IEstablishmentRepository _repository;

    public EstablishmentSearchSuggestions(IEstablishmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<SearchSuggestionsResult<EstablishmentSearchSuggestionsResultDTO>>> HandleRequest(
        EstablishmentSearchSuggestionsRequest request)
    {
        var searchType = request.SearchTerm.ClassifySearchType();
        var maxSuggestions = request.MaxSuggestions ?? Core.Constants.SearchResultMaxSuggestions;

        if (searchType == SearchType.Urn)
        {
            var result = await _repository.GetEstablishmentDetails(request.SearchTerm);

            return result.Map(x => new SearchSuggestionsResult<EstablishmentSearchSuggestionsResultDTO>()
            {
                Suggestions = new EstablishmentSearchSuggestionsResultDTO[]
                {
                    x.MapToSearchSuggestionsResult()
                },
                MaxSuggestions = maxSuggestions,
                SearchTerm = request.SearchTerm,
            });
        }

        var results = await _repository.EstablishmentSearchSuggestions(request.SearchTerm, maxSuggestions);

        return results.Map(x => new SearchSuggestionsResult<EstablishmentSearchSuggestionsResultDTO>()
        {
            Suggestions = x.Suggestions.MapToListOfSearchSuggestionsResultsDTO(),
            MaxSuggestions = maxSuggestions,
            SearchTerm = request.SearchTerm,
        });
    }
}