using ASP.Core.Establishments;
using ASP.Core.Search;
using ASP.Core.Utilities;
using EstablishmentDetailsSearchResultDTO = ASP.Core.Search.EstablishmentDetailsSearchResultDTO;

namespace ASP.Core.Mapper.Establishment;

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
            LaEstab = details.Laestab
        };
    }

    public static EstablishmentDetailsSearchResultDTO MapToSearchResultDTO(
        this EstablishmentDetailsSearchResult details)
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
            LaEstab = details.Laestab
        };
    }

    public static List<EstablishmentDetailsSearchResultDTO> MapToListOfSearchResultsDTO(
        this IEnumerable<EstablishmentDetailsSearchResult> detailsList)
    {
        return detailsList.Select(MapToSearchResultDTO).ToList();
    }
}