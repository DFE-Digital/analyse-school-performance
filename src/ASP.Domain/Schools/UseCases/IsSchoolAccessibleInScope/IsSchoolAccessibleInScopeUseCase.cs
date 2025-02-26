using ASP.Core.Results;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools.UseCases.IsSchoolAccessibleInScope;

public class IsSchoolAccessibleInScopeUseCase : IIsSchoolAccessibleInScopeUseCase
{
    private readonly ISchoolRepository _repository;
    private readonly ISchoolAccessScopeValidator _scopeValidator;

    public IsSchoolAccessibleInScopeUseCase(
        ISchoolRepository pageContentRepository,
        ISchoolAccessScopeValidator scopeValidator)
    {
        _repository = pageContentRepository
            ?? throw new ArgumentNullException(nameof(pageContentRepository));
        _scopeValidator = scopeValidator
            ?? throw new ArgumentNullException(nameof(scopeValidator));
    }

    public Task<Result<IsSchoolAccessibleInScopeResponse>> HandleRequest(IsSchoolAccessibleInScopeRequest request)
    {
        return
            from scope in request.Scope.Then(_scopeValidator.ValidateScope)
            from urn in SchoolUrn.Parse(request.Urn)
            from school in _repository.GetWithLinkedSchools(urn)
            let access = school.GetAccessForScope(scope)
            select new IsSchoolAccessibleInScopeResponse(
                request.Urn,
                request.Scope.Map(s => s.ScopeType.ToString()).GetValueOrDefault(""),
                request.Scope.Map(s => s.ScopeId).GetValueOrDefault(""),
                access.IsAccessibleInScope,
                access.IsAccessibleViaLinkedSchools);
    }
}