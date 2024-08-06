using ASP.Core.Establishments.Search;

namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class SearchResultMapper
{
    public static EstablishmentDetailsSearchResult MapToEstablishmentDetailsSearchResult(this SearchResultDAO details)
    {
        return new EstablishmentDetailsSearchResult(
            details.Urn,
            details.Name,
            details.IsPrimary,
            details.IsSecondary,
            details.IsPost16,
            details.Address.MapToDomainEntityAddress(),
            details.OfstedRating.MapToDomainEntityOfstedRating(),
            details.OfstedLastInspectionDate,
            details.Laestab,
            details.IsDeleted,
            details.IsVisible
        );
    }

    public static List<EstablishmentDetailsSearchResult> MapToEstablishmentDetailsSearchResults(this IEnumerable<SearchResultDAO> detailsList)
    {
        return detailsList.Select(MapToEstablishmentDetailsSearchResult).ToList();
    }
}