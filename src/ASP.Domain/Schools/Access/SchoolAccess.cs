namespace ASP.Domain.Schools.Access
{
    public record SchoolAccess(bool IsAccessibleInScope, bool IsAccessibleViaLinkedSchools)
    {
        public bool IsAccessible => IsAccessibleInScope || IsAccessibleViaLinkedSchools;
    }
}
