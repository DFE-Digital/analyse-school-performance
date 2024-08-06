using ASP.Core.Establishments.SearchSuggestions;

namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class SearchSuggestionsResultMapper
{
    public static EstablishmentSearchSuggestionsResult MapToEstablishmentSearchSuggestionsResult(this SearchSuggestionsResultDAO details)
    {
        return new EstablishmentSearchSuggestionsResult(
            details.Urn,
            details.Name,
            details.Address.MapToDomainEntityAddress(),
            details.Laestab,
            details.IsDeleted,
            details.IsVisible
        );
    }

    public static List<EstablishmentSearchSuggestionsResult> MapToEstablishmentSearchSuggestionsResults(
        this IEnumerable<SearchSuggestionsResultDAO> detailsList)
    {
        return detailsList.Select(MapToEstablishmentSearchSuggestionsResult).ToList();
    }
}