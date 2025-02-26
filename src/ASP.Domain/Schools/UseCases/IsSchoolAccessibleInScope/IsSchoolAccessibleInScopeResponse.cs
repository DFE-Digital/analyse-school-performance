namespace ASP.Domain.Schools.UseCases.IsSchoolAccessibleInScope
{
    public class IsSchoolAccessibleInScopeResponse
    {
        public string Urn { get; }
        public string Scope { get; }
        public string ScopeIdentifier { get; }
        public bool IsAccessibleInScope { get; }
        public bool IsAccessibleViaLinkedSchools { get; }

        public IsSchoolAccessibleInScopeResponse(
            string urn,
            string scope,
            string scopeIdentifier,
            bool isAccessibleInScope,
            bool isAccessibleViaLinkedSchools)
        {
            Urn = urn;
            Scope = scope;
            ScopeIdentifier = scopeIdentifier;
            IsAccessibleInScope = isAccessibleInScope;
            IsAccessibleViaLinkedSchools = isAccessibleViaLinkedSchools;
        }
    }
}
