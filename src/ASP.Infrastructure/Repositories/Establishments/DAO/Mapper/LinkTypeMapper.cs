namespace ASP.Infrastructure.Repositories.Establishments.DAO.Mapper;

public static class LinkTypeMapper
{
    public static Domain.Establishments.LinkType? MapToDomainEntityLinkType(this LinkTypeDAO? linkType)
    {
        if (linkType == null) return null;
        
        return new Domain.Establishments.LinkType(
            linkType.Code,
            linkType.Name
        );
    }

    public static LinkTypeDAO? MapToLinkTypeDAO(this Domain.Establishments.LinkType? linkType)
    {
        if (linkType == null) return null;

        return new LinkTypeDAO(
            linkType.Code,
            linkType.Name
        );
    }
}