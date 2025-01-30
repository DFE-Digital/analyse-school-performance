using ASP.Domain.Establishments.Search;
using ASP.Domain.Establishments.SearchSuggestions;
using ASP.Core.Pagination;
using ASP.Core.Results;

namespace ASP.Domain.Establishments
{
    public interface IEstablishmentRepository
    {
        Task<Result<ScopedResultsPage<EstablishmentListing>>> GetAllEstablishments(
            EstablishmentScope scope, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);
        
        Task<Result<EstablishmentDetails>> GetEstablishmentDetails(string urn);

        Task<Result<bool>> IsEstablishmentVisibleWithinScope(string urn, EstablishmentScope scope,
            CancellationToken cancellationToken = default);
        
        Task<Result<Done>> Create(string contentId, EstablishmentDetails establishmentDetails);

        Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentByLaCode(
            EstablishmentScope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentByEstablishmentNumber(
            EstablishmentScope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<ScopedSearchResultsPage<EstablishmentListing>>>
            SearchEstablishmentByLaestab7DigitCode(
                EstablishmentScope scope, string searchTerm, int page, int resultsPerPage,
                CancellationToken cancellationToken = default);

        Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentByLaCodeOrEstablishmentNumber(
            EstablishmentScope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentNameOrLocation(
            EstablishmentScope scope, string searchTerm, 
            int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<List<EstablishmentSuggestion>>>
            GetEstablishmentSearchSuggestions(
                EstablishmentScope scope, string searchTerm, 
                int maxSuggestions, CancellationToken cancellationToken = default);

        Task<Result<List<EstablishmentSuggestion>>>
            GetEstablishmentSearchSuggestionsByUrn(
                EstablishmentScope scope, string searchTerm,
                int maxSuggestions, CancellationToken cancellationToken = default);

        Task<Result<List<EstablishmentSuggestion>>>
            GetEstablishmentSearchSuggestionsByLaEstab(
                EstablishmentScope scope, string searchTerm,
                int maxSuggestions, CancellationToken cancellationToken = default);

        Task<Result<List<EstablishmentSuggestion>>>
            GetEstablishmentSearchSuggestionsByNameAddress(
                EstablishmentScope scope, string searchTerm,
                int maxSuggestions, CancellationToken cancellationToken = default);
    }
}
