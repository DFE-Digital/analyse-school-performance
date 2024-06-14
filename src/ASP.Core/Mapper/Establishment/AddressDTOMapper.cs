using ASP.Core.DTO.Establishment;
using ASP.Core.Establishments;

namespace ASP.Core.Mapper.Establishment;

public static class AddressDTOMapper
{
    public static AddressDTO MapToAddressDTO(this Address? address)
    {
        if (address == null) return new AddressDTO();
        
        return new AddressDTO()
        {
            Street = address.Street,
            Town = address.Town,
            PostCode = address.PostCode
        };
    }
}