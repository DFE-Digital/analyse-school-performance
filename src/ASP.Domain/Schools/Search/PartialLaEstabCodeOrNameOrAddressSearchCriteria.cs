namespace ASP.Domain.Schools.Search
{
    public record PartialLaEstabCodeOrNameOrAddressSearchCriteria(string RawValue) : ISearchCriteria
    {
    }
}
