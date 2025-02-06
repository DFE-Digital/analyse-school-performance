using ASP.Domain.Establishments.LinkedEstablishments;

namespace ASP.Domain.Repositories.Establishments.DAO.Mapper;

public static class LinkTypeMapper
{
    public static LinkType? MapToDomainEntityLinkType(this LinkTypeDAO? linkType)
    {
        if (linkType == null) return null;

        return new LinkType(
            linkType.Code,
            linkType.Name
        );
    }

    public static LinkTypeDAO? MapToLinkTypeDAO(this LinkType? linkType)
    {
        if (linkType == null) return null;

        return new LinkTypeDAO(
            linkType.Code,
            linkType.Name
        );
    }
}