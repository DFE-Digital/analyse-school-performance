namespace ASP.Infrastructure.Repositories.Establishments.DAO.Mapper;

public static class LinkMapper
{
    public static List<Domain.Establishments.Link>? MapToDomainEntityLinks(this List<LinkDAO>? links)
    {
        if (links == null) return null;

        return links.Select(link => new Domain.Establishments.Link(
            link.EstablishedDate,
            link.LinkType?.MapToDomainEntityLinkType(),
            link.LinkedUrn
        )).ToList();
    }

    public static List<LinkDAO>? MapToLinkDAOs(this List<Domain.Establishments.Link>? links)
    {
        if (links == null) return null;

        return links.Select(link => new LinkDAO(
            link.EstablishedDate,
            link.LinkType?.MapToLinkTypeDAO(),
            link.LinkedUrn
        )).ToList();
    }
}