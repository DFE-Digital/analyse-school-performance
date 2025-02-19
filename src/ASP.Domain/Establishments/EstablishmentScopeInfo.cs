using ASP.Core.Optionality;

namespace ASP.Domain.Establishments;

public record EstablishmentScopeInfo(EstablishmentScopeType ScopeType, string ScopeId)
{
    public static Optional<EstablishmentScopeInfo> Create(Optional<EstablishmentScopeType> scopeType, Optional<string> scopeId)
    {
        return scopeType.Map(
            st => new EstablishmentScopeInfo(st, scopeId.GetValueOrDefault(""))
        );
    }
}