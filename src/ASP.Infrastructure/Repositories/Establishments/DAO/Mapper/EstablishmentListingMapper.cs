using ASP.Domain.Establishments;

namespace ASP.Infrastructure.Repositories.Establishments.DAO.Mapper;

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
            details.Laestab
        );
    }
}