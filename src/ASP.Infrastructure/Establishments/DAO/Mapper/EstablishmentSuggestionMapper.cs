using ASP.Core.Establishments.SearchSuggestions;

namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class EstablishmentSuggestionMapper
{
    public static EstablishmentSuggestion MapToEstablishmentSuggestion(this EstablishmentDAO details)
    {
        return new EstablishmentSuggestion(
            details.Urn,
            details.Name,
            details.Address.MapToDomainEntityAddress(),
            details.Laestab,
            details.IsDeleted,
            details.IsVisible
        );
    }

    public static List<EstablishmentSuggestion> MapToEstablishmentSuggestions(
        this IEnumerable<EstablishmentDAO> detailsList)
    {
        return detailsList.Select(MapToEstablishmentSuggestion).ToList();
    }
}