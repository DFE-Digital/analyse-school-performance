namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class AddressMapper
{
    public static Core.Establishments.Address? MapToDomainEntityAddress(this AddressDAO? address)
    {
        if (address == null) return null;  // Return null directly instead of an empty object

        return new Core.Establishments.Address()
        {
            Street = address.Street,
            Town = address.Town,
            PostCode = address.PostCode
        };
    }

    public static AddressDAO? MapToAddressDAO(this Core.Establishments.Address? address)
    {
        if (address == null) return null;  // Return null directly instead of an empty object
        return new AddressDAO(address.Street, address.Town, address.PostCode);
    }
}