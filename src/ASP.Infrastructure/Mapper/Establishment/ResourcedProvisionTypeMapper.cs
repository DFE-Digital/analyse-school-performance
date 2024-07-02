using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class ResourcedProvisionTypeMapper
{
    public static Core.Establishments.ResourcedProvisionType? MapToDomainEntityResourcedProvisionType(this ResourcedProvisionType? resourcedProvisionType)
    {
        if (resourcedProvisionType == null) return null;  // Return null directly instead of an empty object
        
        return new Core.Establishments.ResourcedProvisionType()
        {
            Code = resourcedProvisionType.Code,
            Name = resourcedProvisionType.Name
        };
    }
    
    public static ResourcedProvisionType? MapToResourcedProvisionTypeDAO(this Core.Establishments.ResourcedProvisionType? resourcedProvisionType)
    {
        if (resourcedProvisionType == null) return null;  // Return null directly instead of an empty object

        return new ResourcedProvisionType(resourcedProvisionType.Code, resourcedProvisionType.Name);
    }
}