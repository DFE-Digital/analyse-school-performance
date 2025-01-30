namespace ASP.Infrastructure.Repositories.Establishments.DAO.Mapper;

public static class AddressMapper
{
    public static Domain.Establishments.Address? MapToDomainEntityAddress(this AddressDAO? address)
    {
        if (address == null) return null;  // Return null directly instead of an empty object

        return new Domain.Establishments.Address(
            address.Street,
            address.Town,
            address.PostCode
        );
    }

    public static AddressDAO? MapToAddressDAO(this Domain.Establishments.Address? address)
    {
        if (address == null) return null;  // Return null directly instead of an empty object

        return new AddressDAO(
            address.Street,
            address.Town,
            address.PostCode
        );
    }
}