using ASP.Core.Establishments.SearchSuggestions;

namespace ASP.Application.UseCases.Establishments.DTO.Mapper;

public static class EstablishmentSuggestionDTOMapper
{
    public static EstablishmentSuggestionDTO MapToEstablishmentSuggestionDTO(
        this EstablishmentSuggestion details)
    {
        return new EstablishmentSuggestionDTO(
        )
        {
            Urn = details.Urn,
            Name = details.Name,
            Address = details.Address.MapToAddressDTO(),
            Laestab = details.Laestab
        };
    }

    public static List<EstablishmentSuggestionDTO> MapToListOfEstablishmentSuggestionDTO(
        this IEnumerable<EstablishmentSuggestion> detailsList)
    {
        return detailsList.Select(MapToEstablishmentSuggestionDTO).ToList();
    }
}