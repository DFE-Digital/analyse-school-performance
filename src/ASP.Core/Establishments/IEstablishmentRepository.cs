using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Core.Search.Suggestions;

namespace ASP.Core.Establishments
{
    public interface IEstablishmentRepository
    {
        Task<Result<EstablishmentDetails>> GetEstablishmentDetails(string urn);
        Task<Result<Done>> Create(string contentId, EstablishmentDetails establishmentDetails);

        Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> SearchEstablishmentByLaCode(
            string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> SearchEstablishmentByEstablishmentNumber(
            string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>>
            SearchEstablishmentByLocalAuthEstablishment7DigitCode(
                string searchTerm, int page, int resultsPerPage,
                CancellationToken cancellationToken = default);

        Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> SearchEstablishmentByLaCodeOrEstablishmentNumber(
            string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> SearchEstablishmentNameOrLocation(
            string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<SearchSuggestionsResult<EstablishmentSearchSuggestionsResult>>>
            EstablishmentSearchSuggestions(
                string searchTerm, int maxSuggestions, CancellationToken cancellationToken = default);
    }
}
