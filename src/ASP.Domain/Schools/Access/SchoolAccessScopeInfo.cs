using ASP.Core.Optionality;

namespace ASP.Domain.Schools.Access;

public record SchoolAccessScopeInfo(SchoolAccessScopeType ScopeType, string ScopeId)
{
    public static Optional<SchoolAccessScopeInfo> Create(Optional<SchoolAccessScopeType> scopeType, Optional<string> scopeId)
    {
        return scopeType.Map(
            st => new SchoolAccessScopeInfo(st, scopeId.GetValueOrDefault(""))
        );
    }
}