using ASP.Core.Results;

namespace ASP.Domain.Schools.Access
{
    public interface ISchoolAccessScopeValidator
    {
        Task<Result<SchoolAccessScope>> ValidateScope(SchoolAccessScopeInfo scope);
    }
}
