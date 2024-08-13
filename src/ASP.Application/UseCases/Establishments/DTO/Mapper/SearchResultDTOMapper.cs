using ASP.Core.Establishments;
using ASP.Core.Utilities;

namespace ASP.Application.UseCases.Establishments.DTO.Mapper;

public static class SearchResultDTOMapper
{
    public static EstablishmentDetailsSearchResultDTO MapToSearchResult(this EstablishmentDetails details)
    {
        return new EstablishmentDetailsSearchResultDTO(
        )
        {
            Urn = details.Urn,
            Name = details.Name,
            EducationPhase = EducationPhase.GetPhaseOfEducation(details.IsPrimary,
                details.IsSecondary, details.IsPost16),
            Address = details.Address.MapToAddressDTO(),
            OfstedRating = details.OfstedRating.MapToOfstedRatingDTO(details.OfstedLastInspectionDate),
            Laestab = details.Laestab
        };
    }

    public static EstablishmentDetailsSearchResultDTO MapToSearchResultDTO(
        this EstablishmentListItem details)
    {
        return new EstablishmentDetailsSearchResultDTO(
        )
        {
            Urn = details.Urn,
            Name = details.Name,
            Address = details.Address.MapToAddressDTO(),
            EducationPhase = EducationPhase.GetPhaseOfEducation(details.IsPrimary,
                details.IsSecondary, details.IsPost16),
            OfstedRating = details.OfstedRating.MapToOfstedRatingDTO(details.OfstedLastInspectionDate),
            Laestab = details.Laestab
        };
    }

    public static List<EstablishmentDetailsSearchResultDTO> MapToListOfSearchResultsDTO(
        this IEnumerable<EstablishmentListItem> detailsList)
    {
        return detailsList.Select(MapToSearchResultDTO).ToList();
    }
}