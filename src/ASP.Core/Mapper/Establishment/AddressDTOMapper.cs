using ASP.Core.Establishments;

namespace ASP.Core.Mapper.Establishment;

public static class AddressDTOMapper
{
    public static string? MapToAddressDTO(this Address? address)
    {
        if (address == null) return null;  // Return null directly instead of an empty object
        
        return address.ToString();
    }
}