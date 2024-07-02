using ASP.Core.DTO.Establishment;
using ASP.Core.Establishments;

namespace ASP.Core.Mapper.Establishment;

public static class ResourcedProvisionTypeDTOMapper
{
    public static ResourcedProvisionTypeDTO? MapToResourcedProvisionTypeDTO(this ResourcedProvisionType? resourcedProvisionType)
    {
        if (resourcedProvisionType == null) return null;  // Return null directly instead of an empty object
        return new ResourcedProvisionTypeDTO()
        {
           Code = resourcedProvisionType.Code,
           Name = resourcedProvisionType.Name
        };
    }
}