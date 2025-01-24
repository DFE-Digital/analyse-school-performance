namespace ASP.Core.Establishments
{
    public class EstablishmentListing
    {
        public string Urn { get; }
        public string Name { get; }
        public bool? IsPrimary { get; }
        public bool? IsSecondary { get; }
        public bool? IsPost16 { get; }
        public Address? Address { get; }
        public string? Laestab { get; }

        public EstablishmentListing(
            string urn,
            string name,
            bool? isPrimary, bool? isSecondary,
            bool? isPost16, Address? address,
            string? laestab)
        {
            Urn = urn;
            Name = name;
            IsPrimary = isPrimary;
            IsSecondary = isSecondary;
            IsPost16 = isPost16;
            Address = address;
            Laestab = laestab;
        }
    }
}