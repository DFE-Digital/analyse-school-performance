using ASP.Core.Search.Suggestions;
using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class SearchSuggestionsResultMapper
{
    public static EstablishmentSearchSuggestionsResult MapToEstablishmentSearchSuggestionsResult(this SearchSuggestionsResultDAO details)
    {
        return new EstablishmentSearchSuggestionsResult()
        {
            Urn = details.Urn,
            Name = details.Name,
            Address = details.Address.MapToDomainEntityAddress(),
            Laestab = details.Laestab,
            IsDeleted = details.IsDeleted,
            IsVisible = details.IsVisible
        };
    }

    public static List<EstablishmentSearchSuggestionsResult> MapToEstablishmentSearchSuggestionsResults(
        this IEnumerable<SearchSuggestionsResultDAO> detailsList)
    {
        return detailsList.Select(MapToEstablishmentSearchSuggestionsResult).ToList();
    }
}