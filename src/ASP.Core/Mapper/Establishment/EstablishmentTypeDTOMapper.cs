using ASP.Core.DTO.Establishment;
using ASP.Core.Establishments;

namespace ASP.Core.Mapper.Establishment;

public static class EstablishmentTypeDTOMapper
{
    public static EstablishmentTypeDTO MapToEstablishmentTypeDTO(this EstablishmentType? establishmentType)
    {
        if (establishmentType == null) return new EstablishmentTypeDTO();
        
        return new EstablishmentTypeDTO()
        {
            Code = establishmentType.Code,
            Name = establishmentType.Name
        };
    }
}