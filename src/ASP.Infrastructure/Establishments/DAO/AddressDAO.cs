namespace ASP.Infrastructure.Establishments.DAO;

public class AddressDAO
{
    public string Street { get; }
    public string Town { get; }
    public string PostCode { get; }

    public AddressDAO(string street, string town, string postCode)
    {
        Street = street;
        Town = town;
        PostCode = postCode;
    }
}