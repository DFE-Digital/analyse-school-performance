using ASP.Core.Optionality;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools.UseCases.IsSchoolAccessibleInScope;

public record IsSchoolAccessibleInScopeRequest(string Urn, Optional<SchoolAccessScopeInfo> Scope);