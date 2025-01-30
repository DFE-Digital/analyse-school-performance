namespace ASP.Domain.Establishments.UseCases.DTO.Mapper;

public static class ResourcedProvisionTypeDTOMapper
{
    public static ResourcedProvisionTypeDTO? MapToResourcedProvisionTypeDTO(this ResourcedProvisionType? resourcedProvisionType)
    {
        if (resourcedProvisionType == null) return null;  // Return null directly instead of an empty object
        return new ResourcedProvisionTypeDTO() {
            Code = resourcedProvisionType.Code,
            Name = resourcedProvisionType.Name
        };
    }
}