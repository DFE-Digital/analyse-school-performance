namespace ASP.Core.Establishments;

public class Scope
{
    public ScopeType ScopeType { get; }
    public string ScopeIdentifier { get; }

    public Scope(ScopeType scopeType, string scopeIdentifier)
    {
        ScopeType = scopeType;
        ScopeIdentifier = scopeIdentifier;
    }
}