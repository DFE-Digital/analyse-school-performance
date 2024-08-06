using ASP.Core.Helpers;

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

        public override string ToString()
        {
            return StringHelper.ConcatNonEmpties(" ", StringHelper.ConcatNonEmpties(", ", Street, Town), PostCode);
        }
    }
}
