using ASP.Core.Establishments;

namespace ASP.Core.Search
{
    public class EstablishmentDetailsSearchResult
    {
        public string Urn { get; set; }
        public string Name { get; set; }
        public bool? IsPrimary { get; set; }
        public bool? IsSecondary { get; set; }
        public bool? IsPost16 { get; set; }
        public Address Address { get; set; }
        public OfstedRating OfstedRating { get; set; }
        public DateTime? OfstedLastInspectionDate { get; set; }
        public string? Laestab { get; set; }
        public bool IsDeleted { get; set; }
    }
}