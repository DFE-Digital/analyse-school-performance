using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class AddressMapper
{
    public static Core.Establishments.Address MapToDomainEntityAddress(this Address? address)
    {
        if (address == null) return new Core.Establishments.Address();
        
        return new Core.Establishments.Address()
        {
            Street = address.Street,
            Town = address.Town,
            PostCode = address.PostCode
        };
    }
    
    public static Address MapToAddressDAO(this Core.Establishments.Address address)
    {
        return new Address(address.Street, address.Town, address.PostCode);
    }
}