namespace ASP.Domain.Establishments.UseCases.IsEstablishmentAccessibleInScope
{
    public class IsEstablishmentAccessibleInScopeResponse
    {
        public string Urn { get; } = "";
        public string Scope { get; } = "";
        public string ScopeIdentifier { get; } = "";
        public bool IsAccessible { get; }

        public IsEstablishmentAccessibleInScopeResponse(string urn, string scope, string scopeIdentifier, bool isAccessible)
        {
            Urn = urn;
            Scope = scope;
            ScopeIdentifier = scopeIdentifier;
            IsAccessible = isAccessible;
        }
    }
}
