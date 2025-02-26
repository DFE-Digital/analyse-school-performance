namespace ASP.Domain.Repositories.Schools;

public class AddressDao
{
    public string Street { get; }
    public string Town { get; }
    public string PostCode { get; }

    public AddressDao(string street, string town, string postCode)
    {
        Street = street;
        Town = town;
        PostCode = postCode;
    }
}