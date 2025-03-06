namespace ASP.Domain.LocalAuthorities
{
    public record PartialCodeSearchCriteria(string RawValue) : ILocalAuthoritySearchCriteria
    {
    }
}
