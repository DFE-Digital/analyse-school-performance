using ASP.Application.UseCases.LocalAuthorities.DTO;
using ASP.Application.UseCases.LocalAuthorities.DTO.Mapper;
using ASP.Core.LocalAuthorities;
using ASP.Core.LocalAuthorities.LocalAuthoritySearchSuggestions;
using ASP.Core.Results;

namespace ASP.Application.UseCases.LocalAuthorities.LocalAuthoritySearchSuggestions;

public class LocalAuthoritySearchSuggestions : ILocalAuthoritySearchSuggestions
{
    private readonly ILocalAuthorityRepository _repository;

    public LocalAuthoritySearchSuggestions(ILocalAuthorityRepository repository)
    {
        _repository = repository ??
                      throw new ArgumentNullException(nameof(repository));
    }

    public Task<Result<LocalAuthoritySearchSuggestionsResult<LocalAuthorityDTO>>> HandleRequest(LocalAuthoritySearchSuggestionsRequest request)
    {
        var maxSuggestions = request.MaxSuggestions.GetValueOrDefault(Core.Constants.SearchResultMaxSuggestions);
        
        var isNumeric = int.TryParse(request.SearchTerm, out var _);

        var suggestions = isNumeric
            ? _repository.LocalAuthoritySearchSuggestionsByLaCode(request.SearchTerm, maxSuggestions)
            : _repository.LocalAuthoritySearchSuggestionsByLaName(request.SearchTerm, maxSuggestions);
        
        return from response in suggestions
        select new LocalAuthoritySearchSuggestionsResult<LocalAuthorityDTO> {
            Suggestions = response.MapToLocalAuthorityDTO(),
            MaxSuggestions = maxSuggestions,
            SearchTerm = request.SearchTerm
        };
    }
}