using ASP.Core.Establishments;

namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class EstablishmentListingMapper
{
    public static EstablishmentListing MapToEstablishmentListing(this EstablishmentDAO details)
    {
        return new EstablishmentListing(
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
}