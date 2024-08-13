using ASP.Core.Establishments.Search;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Results;

namespace ASP.Core.Establishments
{
    public interface IEstablishmentRepository
    {
        Task<Result<EstablishmentDetails>> GetEstablishmentDetails(string urn);
        Task<Result<Done>> Create(string contentId, EstablishmentDetails establishmentDetails);

        Task<Result<SearchResultsPage<EstablishmentListItem>>> SearchEstablishmentByLaCode(
            Scope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<SearchResultsPage<EstablishmentListItem>>> SearchEstablishmentByEstablishmentNumber(
            Scope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<SearchResultsPage<EstablishmentListItem>>>
            SearchEstablishmentByLaestab7DigitCode(
                Scope scope, string searchTerm, int page, int resultsPerPage,
                CancellationToken cancellationToken = default);

        Task<Result<SearchResultsPage<EstablishmentListItem>>> SearchEstablishmentByLaCodeOrEstablishmentNumber(
            Scope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<SearchResultsPage<EstablishmentListItem>>> SearchEstablishmentNameOrLocation(
            Scope scope, string searchTerm, 
            int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<SearchSuggestionsResult<EstablishmentSearchSuggestionsResult>>>
            EstablishmentSearchSuggestions(
                Scope scope, string searchTerm, 
                int maxSuggestions, CancellationToken cancellationToken = default);
    }
}
