using ASP.Core.Establishments;
using ASP.Core.Search;
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
            IsPrimary = details.IsPrimary,
            IsSecondary = details.IsSecondary,
            IsPost16 = details.IsPost16,
            Address = details.Address.MapToAddressDTO(),
            OfstedRating = details.OfstedRating.MapToOfstedRatingDTO(),
            OfstedLastInspectionDate = details.OfstedLastInspectionDate,
            LaEstab = details.Laestab,
            IsDeleted = details.IsDeleted
        };
    }
    
    public static EstablishmentDetailsSearchResultDTO MapToSearchResultDTO(this EstablishmentDetailsSearchResult details)
    {
        return new EstablishmentDetailsSearchResultDTO(
        )
        {
            Urn = details.Urn,
            Name = details.Name,
            IsPrimary = details.IsPrimary,
            IsSecondary = details.IsSecondary,
            IsPost16 = details.IsPost16,
            Address = details.Address.MapToAddressDTO(),
            OfstedRating = details.OfstedRating.MapToOfstedRatingDTO(),
            OfstedLastInspectionDate = details.OfstedLastInspectionDate,
            LaEstab = details.Laestab,
            IsDeleted = details.IsDeleted
        };
    }
    
    public static List<EstablishmentDetailsSearchResultDTO> MapToListOfSearchResultsDTO(this IEnumerable<EstablishmentDetailsSearchResult> detailsList)
    {
        return detailsList.Select(MapToSearchResultDTO).ToList();
    }
}