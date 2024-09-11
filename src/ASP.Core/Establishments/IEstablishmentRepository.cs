using ASP.Core.Establishments.Search;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Results;
using ASP.Core.Scope;

namespace ASP.Core.Establishments
{
    public interface IEstablishmentRepository
    {
        Task<Result<ScopedResultsPage<EstablishmentListing>>> GetAllEstablishments(
            Scope.Scope scope, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);
        Task<Result<EstablishmentDetails>> GetEstablishmentDetails(string urn);
        Task<Result<Done>> Create(string contentId, EstablishmentDetails establishmentDetails);

        Task<Result<SearchResultsPage<EstablishmentListing>>> SearchEstablishmentByLaCode(
            Scope.Scope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<SearchResultsPage<EstablishmentListing>>> SearchEstablishmentByEstablishmentNumber(
            Scope.Scope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<SearchResultsPage<EstablishmentListing>>>
            SearchEstablishmentByLaestab7DigitCode(
                Scope.Scope scope, string searchTerm, int page, int resultsPerPage,
                CancellationToken cancellationToken = default);

        Task<Result<SearchResultsPage<EstablishmentListing>>> SearchEstablishmentByLaCodeOrEstablishmentNumber(
            Scope.Scope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<SearchResultsPage<EstablishmentListing>>> SearchEstablishmentNameOrLocation(
            Scope.Scope scope, string searchTerm, 
            int page, int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<List<EstablishmentSuggestion>>>
            GetEstablishmentSearchSuggestions(
                Scope.Scope scope, string searchTerm, 
                int maxSuggestions, CancellationToken cancellationToken = default);

        Task<Result<List<EstablishmentSuggestion>>>
            GetEstablishmentSearchSuggestionsByUrn(
                Scope.Scope scope, string searchTerm,
                int maxSuggestions, CancellationToken cancellationToken = default);

        Task<Result<List<EstablishmentSuggestion>>>
            GetEstablishmentSearchSuggestionsByLaEstab(
                Scope.Scope scope, string searchTerm,
                int maxSuggestions, CancellationToken cancellationToken = default);

        Task<Result<List<EstablishmentSuggestion>>>
            GetEstablishmentSearchSuggestionsByNameAddress(
                Scope.Scope scope, string searchTerm,
                int maxSuggestions, CancellationToken cancellationToken = default);
    }
}
