namespace ASP.Domain.Schools.Search
{
    public record PartialLAEstabCodeSearchCriteria(string RawValue) : ISearchCriteria
    {
        public string SearchTerm => RawValue;
    }
}
