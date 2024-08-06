namespace ASP.Core.Establishments.Search
{
    public class EstablishmentDetailsSearchResult
    {
        public string Urn { get; }
        public string Name { get; }
        public bool? IsPrimary { get; }
        public bool? IsSecondary { get; }
        public bool? IsPost16 { get; }
        public Address? Address { get; }
        public OfstedRating? OfstedRating { get; }
        public DateTime? OfstedLastInspectionDate { get; }
        public string? Laestab { get; }
        public bool IsDeleted { get; }
        public bool IsVisible { get; }

        public EstablishmentDetailsSearchResult(string urn, string name, bool? isPrimary, bool? isSecondary, bool? isPost16, Address? address, OfstedRating? ofstedRating, DateTime? ofstedLastInspectionDate, string? laestab, bool isDeleted, bool isVisible)
        {
            Urn = urn;
            Name = name;
            IsPrimary = isPrimary;
            IsSecondary = isSecondary;
            IsPost16 = isPost16;
            Address = address;
            OfstedRating = ofstedRating;
            OfstedLastInspectionDate = ofstedLastInspectionDate;
            Laestab = laestab;
            IsDeleted = isDeleted;
            IsVisible = isVisible;
        }
    }
}