using ASP.Core.DTO.Establishment;
using ASP.Core.Establishments;

namespace ASP.Core.Mapper.Establishment;

public static class EstablishmentTypeDTOMapper
{
    public static EstablishmentTypeDTO? MapToEstablishmentTypeDTO(this EstablishmentType? establishmentType)
    {
        if (establishmentType == null) return null;  // Return null directly instead of an empty object
        
        return new EstablishmentTypeDTO()
        {
            Code = establishmentType.Code,
            Name = establishmentType.Name
        };
    }
}