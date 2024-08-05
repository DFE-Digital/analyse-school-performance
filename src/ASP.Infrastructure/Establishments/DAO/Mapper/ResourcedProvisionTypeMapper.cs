namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class ResourcedProvisionTypeMapper
{
    public static Core.Establishments.ResourcedProvisionType? MapToDomainEntityResourcedProvisionType(this ResourcedProvisionTypeDAO? resourcedProvisionType)
    {
        if (resourcedProvisionType == null) return null;  // Return null directly instead of an empty object

        return new Core.Establishments.ResourcedProvisionType()
        {
            Code = resourcedProvisionType.Code,
            Name = resourcedProvisionType.Name
        };
    }

    public static ResourcedProvisionTypeDAO? MapToResourcedProvisionTypeDAO(this Core.Establishments.ResourcedProvisionType? resourcedProvisionType)
    {
        if (resourcedProvisionType == null) return null;  // Return null directly instead of an empty object

        return new ResourcedProvisionTypeDAO(resourcedProvisionType.Code, resourcedProvisionType.Name);
    }
}