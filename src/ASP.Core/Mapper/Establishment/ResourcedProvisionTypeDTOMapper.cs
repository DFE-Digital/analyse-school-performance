using ASP.Core.DTO.Establishment;
using ASP.Core.Establishments;

namespace ASP.Core.Mapper.Establishment;

public static class ResourcedProvisionTypeDTOMapper
{
    public static ResourcedProvisionTypeDTO MapToResourcedProvisionTypeDTO(this ResourcedProvisionType? resourcedProvisionType)
    {
        if (resourcedProvisionType == null) return new ResourcedProvisionTypeDTO();
        return new ResourcedProvisionTypeDTO()
        {
           Code = resourcedProvisionType.Code,
           Name = resourcedProvisionType.Name
        };
    }
}