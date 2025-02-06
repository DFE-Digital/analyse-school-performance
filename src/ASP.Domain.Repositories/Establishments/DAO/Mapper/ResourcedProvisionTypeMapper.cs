namespace ASP.Domain.Repositories.Establishments.DAO.Mapper;

public static class ResourcedProvisionTypeMapper
{
    public static Domain.Establishments.ResourcedProvisionType? MapToDomainEntityResourcedProvisionType(this ResourcedProvisionTypeDAO? resourcedProvisionType)
    {
        if (resourcedProvisionType == null) return null;  // Return null directly instead of an empty object

        return new Domain.Establishments.ResourcedProvisionType(
            resourcedProvisionType.Code,
            resourcedProvisionType.Name
        );
    }

    public static ResourcedProvisionTypeDAO? MapToResourcedProvisionTypeDAO(this Domain.Establishments.ResourcedProvisionType? resourcedProvisionType)
    {
        if (resourcedProvisionType == null) return null;  // Return null directly instead of an empty object

        return new ResourcedProvisionTypeDAO(
            resourcedProvisionType.Code,
            resourcedProvisionType.Name
        );
    }
}