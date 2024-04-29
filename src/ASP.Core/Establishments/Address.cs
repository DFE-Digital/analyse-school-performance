namespace ASP.Core.Establishments
{
    public class Address
    {
        public string Street { get; }
        public string Town { get; }
        public string PostCode { get; }

        public Address(string street, string town, string postCode)
        {
            Street = street;
            Town = town;
            PostCode = postCode;
        }
    }
}
