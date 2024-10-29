using ASP.Core.Establishments.Search;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Results;
using ASP.Core.Scoping;

namespace ASP.Core.Establishments
{
    public interface IEstablishmentRepository
    {
        Task<Result<ScopedResultsPage<EstablishmentListing>>> GetAllEstablishments(
            Scope scope, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);
        Task<Result<EstablishmentDetails>> GetEstablishmentDetails(string urn);
        Task<Result<Done>> Create(string contentId, EstablishmentDetails establishmentDetails);

        Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentByLaCode(
            Scope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentByEstablishmentNumber(
            Scope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<ScopedSearchResultsPage<EstablishmentListing>>>
            SearchEstablishmentByLaestab7DigitCode(
                Scope scope, string searchTerm, int page, int resultsPerPage,
                CancellationToken cancellationToken = default);

        Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentByLaCodeOrEstablishmentNumber(
            Scope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentNameOrLocation(
            Scope scope, string searchTerm, 
            int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<List<EstablishmentSuggestion>>>
            GetEstablishmentSearchSuggestions(
                Scope scope, string searchTerm, 
                int maxSuggestions, CancellationToken cancellationToken = default);

        Task<Result<List<EstablishmentSuggestion>>>
            GetEstablishmentSearchSuggestionsByUrn(
                Scope scope, string searchTerm,
                int maxSuggestions, CancellationToken cancellationToken = default);

        Task<Result<List<EstablishmentSuggestion>>>
            GetEstablishmentSearchSuggestionsByLaEstab(
                Scope scope, string searchTerm,
                int maxSuggestions, CancellationToken cancellationToken = default);

        Task<Result<List<EstablishmentSuggestion>>>
            GetEstablishmentSearchSuggestionsByNameAddress(
                Scope scope, string searchTerm,
                int maxSuggestions, CancellationToken cancellationToken = default);
    }
}
