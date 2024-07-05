using ASP.Core.Establishments;
using ASP.Core.Search.Suggestions;

namespace ASP.Core.Mapper.Establishment;

public static class SearchSuggestionsResultDTOMapper
{
    public static EstablishmentSearchSuggestionsResultDTO MapToSearchSuggestionsResult(this EstablishmentDetails details)
    {
        return new EstablishmentSearchSuggestionsResultDTO(
        )
        {
            Urn = details.Urn,
            Name = details.Name,
            Address = details.Address.MapToAddressDTO(),
            Laestab = details.Laestab
        };
    }
    
    public static EstablishmentSearchSuggestionsResultDTO MapToSearchSuggestionsResultDTO(
        this EstablishmentSearchSuggestionsResult details)
    {
        return new EstablishmentSearchSuggestionsResultDTO(
        )
        {
            Urn = details.Urn,
            Name = details.Name,
            Address = details.Address.MapToAddressDTO(),
            Laestab = details.Laestab
        };
    }

    public static List<EstablishmentSearchSuggestionsResultDTO> MapToListOfSearchSuggestionsResultsDTO(
        this IEnumerable<EstablishmentSearchSuggestionsResult> detailsList)
    {
        return detailsList.Select(MapToSearchSuggestionsResultDTO).ToList();
    }
}