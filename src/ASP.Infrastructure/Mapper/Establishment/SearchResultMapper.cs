using ASP.Core.Search;
using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class SearchResultMapper
{
    public static EstablishmentDetailsSearchResult MapToEstablishmentDetailsSearchResult(this SearchResultDAO details)
    {
        return new EstablishmentDetailsSearchResult()
        {
            Urn = details.Urn,
            Name = details.Name,
            IsPrimary = details.IsPrimary,
            IsSecondary = details.IsSecondary,
            IsPost16 = details.IsPost16,
            Address = details.Address.MapToDomainEntityAddress(),
            OfstedRating = details.OfstedRating.MapToDomainEntityOfstedRating(),
            OfstedLastInspectionDate = details.OfstedLastInspectionDate,
            Laestab = details.Laestab,
            IsDeleted = details.IsDeleted,
            IsVisible = details.IsVisible
        };
    }

    public static List<EstablishmentDetailsSearchResult> MapToEstablishmentDetailsSearchResults(
        this IEnumerable<SearchResultDAO> detailsList)
    {
        return detailsList.Select(MapToEstablishmentDetailsSearchResult).ToList();
    }
}