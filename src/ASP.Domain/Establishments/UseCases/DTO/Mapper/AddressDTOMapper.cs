namespace ASP.Domain.Establishments.UseCases.DTO.Mapper;

public static class AddressDTOMapper
{
    public static string? MapToAddressDTO(this Address? address)
    {
        if (address == null) return null;  // Return null directly instead of an empty object

        return address.ToString();
    }
}