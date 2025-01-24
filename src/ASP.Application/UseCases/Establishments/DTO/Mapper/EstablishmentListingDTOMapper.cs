using ASP.Core.Establishments;
using ASP.Core.Utilities;

namespace ASP.Application.UseCases.Establishments.DTO.Mapper;

public static class EstablishmentListingDTOMapper
{
    public static EstablishmentListingDTO MapToEstablishmentListingDTO(
        this EstablishmentListing details)
    {
        return new EstablishmentListingDTO()
        {
            Urn = details.Urn,
            Name = details.Name,
            Address = details.Address.MapToAddressDTO(),
            EducationPhase = EducationPhase.GetPhaseOfEducation(details.IsPrimary, details.IsSecondary, details.IsPost16),
            Laestab = details.Laestab
        };
    }
}