using ASP.Domain.Establishments.LinkedEstablishments;

namespace ASP.Domain.Repositories.Establishments.DAO.Mapper;

public static class LinkMapper
{
    public static List<Link>? MapToDomainEntityLinks(this List<LinkDAO>? links)
    {
        if (links == null) return null;

        return links.Select(link => new Link(
            link.EstablishedDate,
            link.LinkType?.MapToDomainEntityLinkType(),
            link.LinkedUrn
        )).ToList();
    }

    public static List<LinkDAO>? MapToLinkDAOs(this List<Link>? links)
    {
        if (links == null) return null;

        return links.Select(link => new LinkDAO(
            link.EstablishedDate,
            link.LinkType?.MapToLinkTypeDAO(),
            link.LinkedUrn
        )).ToList();
    }
}