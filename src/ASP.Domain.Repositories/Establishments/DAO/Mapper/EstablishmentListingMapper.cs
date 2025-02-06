using ASP.Domain.Establishments;

namespace ASP.Domain.Repositories.Establishments.DAO.Mapper;

public static class EstablishmentListingMapper
{
    public static EstablishmentListing MapToEstablishmentListing(this EstablishmentDAO details)
    {
        return new EstablishmentListing(
            details.Urn,
            details.Name,
            new EducationPhase(
                details.IsPrimary,
                details.IsSecondary,
                details.IsPost16),
            details.Address.MapToDomainEntityAddress(),
            details.Laestab
        );
    }
}