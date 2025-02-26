using ASP.Core.Results;

namespace ASP.Domain.Schools.Search
{
    public record FullSchoolUrnSearchCriteria : ISearchCriteria
    {
        public SchoolUrn Urn { get; set; }
        public string RawValue => Urn.Value;

        private FullSchoolUrnSearchCriteria(SchoolUrn urn)
        {
            Urn = urn;
        }

        public static Result<FullSchoolUrnSearchCriteria> Parse(string stringValue)
        {
            return SchoolUrn.Parse(stringValue)
                .Map(urn => new FullSchoolUrnSearchCriteria(urn));
        }
    }
}
