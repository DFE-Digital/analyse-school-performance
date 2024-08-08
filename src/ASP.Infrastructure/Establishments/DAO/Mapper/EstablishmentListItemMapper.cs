using ASP.Core.Establishments.Search;

namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class EstablishmentListItemMapper
{
    public static EstablishmentListItem MapToEstablishmentListItem(this EstablishmentDAO details)
    {
        return new EstablishmentListItem(
            details.Urn,
            details.Name,
            details.IsPrimary,
            details.IsSecondary,
            details.IsPost16,
            details.Address.MapToDomainEntityAddress(),
            details.OfstedRating.MapToDomainEntityOfstedRating(),
            details.OfstedLastInspectionDate,
            details.Laestab
        );
    }

    public static List<EstablishmentListItem> MapToEstablishmentListItem(this IEnumerable<EstablishmentDAO> detailsList)
    {
        return detailsList.Select(MapToEstablishmentListItem).ToList();
    }
}