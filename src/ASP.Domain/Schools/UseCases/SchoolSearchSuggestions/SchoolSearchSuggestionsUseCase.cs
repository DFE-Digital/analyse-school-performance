using ASP.Core.Results;
using ASP.Domain.Schools.Access;
using ASP.Domain.Schools.Search;

namespace ASP.Domain.Schools.UseCases.SchoolSearchSuggestions;

public class SchoolSearchSuggestionsUseCase : ISchoolSearchSuggestionsUseCase
{
    private readonly ISchoolAccessScopeValidator _scopeValidator;
    private readonly SearchSuggestionsService _searchService;

    public SchoolSearchSuggestionsUseCase(
        ISchoolRepository repository,
        ISchoolAccessScopeValidator scopeValidator)
    {

        _scopeValidator = scopeValidator;
        _searchService = new SearchSuggestionsService(repository);
    }

    public Task<Result<List<School>>> HandleRequest(SchoolSearchSuggestionsRequest request)
    {
        var maxSuggestions = request.MaxSuggestions.GetValueOrDefault(Core.Constants.SearchResultMaxSuggestions);

        return
            from scope in request.Scope.Then(_scopeValidator.ValidateScope)
            from response in _searchService.Search(request.SearchTerm, scope, maxSuggestions)
            select response;
    }
}