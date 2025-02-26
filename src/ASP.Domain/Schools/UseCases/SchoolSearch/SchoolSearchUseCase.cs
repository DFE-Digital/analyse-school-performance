using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.Schools.Access;
using ASP.Domain.Schools.Search;

namespace ASP.Domain.Schools.UseCases.SchoolSearch;

public class SchoolSearchUseCase : ISchoolSearchUseCase
{
    private readonly ISchoolAccessScopeValidator _scopeValidator;
    private readonly SearchService _searchService;

    public SchoolSearchUseCase(
        ISchoolRepository repository,
        ISchoolAccessScopeValidator scopeValidator
    )
    {
        _scopeValidator = scopeValidator;
        _searchService = new SearchService(repository);
    }

    public Task<Result<ResultsPage<School>>> HandleRequest(SchoolSearchRequest request)
    {
        var page = request.Page.GetValueOrDefault(1);
        var resultsPerPage = request.ResultsPerPage.GetValueOrDefault(Core.Constants.SearchResultPageSize);
        return
            from scope in request.Scope.Then(_scopeValidator.ValidateScope)
            from results in _searchService.Search(request.SearchTerm, scope, page, resultsPerPage)
            select results;
    }
}