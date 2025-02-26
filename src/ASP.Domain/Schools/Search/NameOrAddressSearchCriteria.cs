namespace ASP.Domain.Schools.Search
{
    public record NameOrAddressSearchCriteria(string RawValue) : ISearchCriteria
    {
    }
}
