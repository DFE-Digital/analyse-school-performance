using ASP.Core;
using ASP.Core.Establishments;
using ASP.Core.Establishments.Search;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Extensions;
using ASP.Core.Results;
using ASP.Infrastructure.Establishments.DAO;
using ASP.Infrastructure.Establishments.DAO.Mapper;

namespace ASP.Infrastructure.Establishments
{
    public class EstablishmentRepository : IEstablishmentRepository
    {
        private const string ContainerKey = "establishments";
        private readonly IDocumentDatabase _documentDB;

        public EstablishmentRepository(IDocumentDatabase documentDB)
        {
            _documentDB = documentDB ??
                          throw new ArgumentNullException(nameof(documentDB));
        }

        public Task<Result<EstablishmentDetails>> GetEstablishmentDetails(string urn)
        {
            return _documentDB.GetAsync<EstablishmentDetailsDAO>(ContainerKey, urn, urn)
                .ErrorIf(estab => estab.IsDeleted, Error.NotFound($@"The requested establishment with URN ""{urn}"" has been deleted."))
                .ErrorIf(estab => !estab.IsVisible, Error.NotFound($@"The requested establishment with URN ""{urn}"" is not currently visible."))
                .Map(dto => dto.MapToEstablishmentDetails());
        }

        public Task<Result<Done>> Create(string contentId, EstablishmentDetails establishmentDetails)
        {
            var establishmentDetailsDao = establishmentDetails.MapToEstablishmentDetailsDAO();

            return _documentDB.UpsertAsync(ContainerKey, contentId, establishmentDetailsDao.Urn,
                establishmentDetailsDao);
        }

        public async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> SearchEstablishmentNameOrLocation(
            string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            return await _documentDB.QueryPagedAsync<SearchResultDAO>(
                    ContainerKey,
                    q => q.Where(x => !x.IsDeleted && x.IsVisible && (
                            x.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                            x.Address != null && (
                                x.Address.Street.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                x.Address.Town.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                x.Address.PostCode.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                            )
                        )).OrderBy(x => x.Name),
                    page,
                    resultsPerPage,
                    cancellationToken)
                .ErrorIf(q => q.TotalResults == 0, Error.NotFound($@"there were no matches for ""{searchTerm}""."))
                .Map(results => new SearchResultsPage<EstablishmentDetailsSearchResult>(searchTerm, results.Map(r => r.MapToEstablishmentDetailsSearchResult())));
        }

        public async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> SearchEstablishmentByLaCode(
            string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            var inputSearchTerm = searchTerm + "/";

            return await SearchLocalAuthEstablishmentCommon(inputSearchTerm, searchTerm, page, resultsPerPage, cancellationToken);
        }

        public async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> SearchEstablishmentByEstablishmentNumber(
            string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            var inputSearchTerm = "/" + searchTerm;

            return await SearchLocalAuthEstablishmentCommon(inputSearchTerm, searchTerm, page, resultsPerPage, cancellationToken);
        }

        public async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> SearchEstablishmentByLaCodeOrEstablishmentNumber(
            string searchTerm,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default
        )
        {
            return await SearchLocalAuthEstablishmentCommon(searchTerm, searchTerm, page, resultsPerPage, cancellationToken);
        }

        public async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> SearchEstablishmentByLocalAuthEstablishment7DigitCode(
            string searchTerm,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default
        )
        {
            var inputSearchTerm = searchTerm.ToLaEstabCodeFormat();

            return await SearchLocalAuthEstablishmentCommon(inputSearchTerm, searchTerm, page, resultsPerPage, cancellationToken);
        }

        public async Task<Result<SearchSuggestionsResult<EstablishmentSearchSuggestionsResult>>> EstablishmentSearchSuggestions(
            string searchTerm,
            int maxSuggestions,
            CancellationToken cancellationToken = default
        )
        {
            return await _documentDB.QueryAsync<SearchSuggestionsResultDAO>(ContainerKey,
                    q => q.Where(x =>
                            !x.IsDeleted && x.IsVisible &&
                            (
                                x.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                x.Address != null && (
                                    x.Address.Street.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.Address.Town.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.Address.PostCode.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                ) ||
                                x.Urn.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                x.Laestab != null && (
                                    x.Laestab.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.Laestab.Replace("/", "").Contains(searchTerm,
                                        StringComparison.CurrentCultureIgnoreCase)
                                )
                            ))
                        .OrderBy(x => x.Name)
                        .Take(maxSuggestions), cancellationToken)
                .ErrorIf(q => !q.Any(), Error.NotFound($@"there were no matches for ""{searchTerm}""."))
                .Map(x => new SearchSuggestionsResult<EstablishmentSearchSuggestionsResult>
                {
                    Suggestions = x.MapToEstablishmentSearchSuggestionsResults(),
                    SearchTerm = searchTerm,
                    MaxSuggestions = maxSuggestions
                });
        }

        private async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> SearchLocalAuthEstablishmentCommon(
            string searchTerm,
            string originalSearchTerm,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default
        )
        {
            return await _documentDB.QueryPagedAsync<SearchResultDAO>(
                    ContainerKey,
                    q => q.Where(x =>
                            !x.IsDeleted && x.IsVisible &&
                            x.Laestab != null && // Ensure Laestab is not null before calling Contains
                            x.Laestab.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                         ).OrderBy(x => x.Name),
                    page,
                    resultsPerPage,
                    cancellationToken)
                .ErrorIf(q => q.TotalResults == 0, Error.NotFound($@"there were no matches for ""{originalSearchTerm}""."))
                .Map(results => new SearchResultsPage<EstablishmentDetailsSearchResult>(originalSearchTerm, results.Map(r => r.MapToEstablishmentDetailsSearchResult())));
        }
    }
}