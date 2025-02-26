namespace ASP.Domain.Schools.Search
{
    public record PartialSchoolUrnSearchCriteria(string RawValue) : ISearchCriteria
    {
        public string SearchTerm => RawValue;
    }
}
