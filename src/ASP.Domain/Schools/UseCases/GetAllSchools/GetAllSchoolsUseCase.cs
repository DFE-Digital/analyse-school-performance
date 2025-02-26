using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools.UseCases.GetAllSchools;

public class GetAllSchoolsUseCase : IGetAllSchoolsUseCase
{
    private readonly ISchoolRepository _repository;
    private readonly ISchoolAccessScopeValidator _scopeValidator;

    public GetAllSchoolsUseCase(
        ISchoolRepository repository,
        ISchoolAccessScopeValidator scopeValidator)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _scopeValidator = scopeValidator ?? throw new ArgumentNullException(nameof(scopeValidator));
    }

    public Task<Result<ResultsPage<School>>> HandleRequest(GetAllSchoolsRequest request)
    {
        var page = request.Page.GetValueOrDefault(1);
        var resultsPerPage = request.ResultsPerPage.GetValueOrDefault(Core.Constants.SearchResultPageSize);

        return
            from scope in request.Scope.Then(_scopeValidator.ValidateScope)
            from results in _repository.GetAll(scope, page, resultsPerPage)
            select results;
    }
}