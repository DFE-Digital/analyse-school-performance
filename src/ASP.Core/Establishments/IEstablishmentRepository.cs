using ASP.Core.Results;
using ASP.Core.Search;

namespace ASP.Core.Establishments
{
    public interface IEstablishmentRepository
    {
        Task<Result<EstablishmentDetails>> GetEstablishmentDetails(string urn);
        Task<Result<Done>> Create(string contentId, EstablishmentDetails establishmentDetails);

        Task<Result<SearchResult<EstablishmentDetailsSearchResult>>> SearchLocalAuthEstablishment(string searchTerm,
            int skip, int take,
            CancellationToken cancellationToken = default);

        Task<Result<SearchResult<EstablishmentDetailsSearchResult>>> SearchEstablishmentNameOrLocation(
            string searchTerm, int skip, int take,
            CancellationToken cancellationToken = default);
    }
}
