using ASP.Core;
using ASP.Core.Establishments;
using ASP.Core.Extensions;
using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Core.Search.Suggestions;
using ASP.Infrastructure.DAO.Establishment;
using ASP.Infrastructure.Mapper.Establishment;

namespace ASP.Infrastructure.Repositories
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

        public async Task<Result<SearchResult<EstablishmentDetailsSearchResult>>> SearchEstablishmentNameOrLocation(
            string searchTerm, int skip, int take,
            CancellationToken cancellationToken = default)
        {
            return await _documentDB.QueryAsyncPaged<SearchResultDAO>(ContainerKey,
                    q => q.Where(x =>
                        !x.IsDeleted && x.IsVisible &&
                        (
                            x.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                            (x.Address != null && (
                                x.Address.Street.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                x.Address.Town.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                x.Address.PostCode.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                            ))
                        )).OrderBy(x => x.Name), skip, take,
                    cancellationToken)
                .ErrorIf(q => q.TotalCount == 0, Error.NotFound($@"there were no matches for ""{searchTerm}""."))
                .Map(x => new SearchResult<EstablishmentDetailsSearchResult>
                {
                    Results = x.Items.MapToEstablishmentDetailsSearchResults(),
                    SearchTerm = searchTerm,
                    TotalResults = x.TotalCount,
                    ResultsPerPage = take
                });
        }
        
        public async Task<Result<SearchResult<EstablishmentDetailsSearchResult>>> SearchEstablishmentByLaCode(
            string searchTerm, int skip, int take,
            CancellationToken cancellationToken = default)
        {
            var inputSearchTerm = searchTerm + "/";
            return await SearchLocalAuthEstablishmentCommon(inputSearchTerm, skip, take, cancellationToken);
        }
        
        public async Task<Result<SearchResult<EstablishmentDetailsSearchResult>>> SearchEstablishmentByEstablishmentNumber(
            string searchTerm, int skip, int take,
            CancellationToken cancellationToken = default)
        {
            var inputSearchTerm = "/" + searchTerm;
            return await SearchLocalAuthEstablishmentCommon(inputSearchTerm, skip, take, cancellationToken);
        }
        
        public async Task<Result<SearchResult<EstablishmentDetailsSearchResult>>> SearchEstablishmentByLaCodeOrEstablishmentNumber(
            string searchTerm, int skip, int take,
            CancellationToken cancellationToken = default)
        {
            return await SearchLocalAuthEstablishmentCommon(searchTerm, skip, take, cancellationToken);
        }
        
        public async Task<Result<SearchResult<EstablishmentDetailsSearchResult>>> SearchEstablishmentByLocalAuthEstablishment7DigitCode(
            string searchTerm, int skip, int take,
            CancellationToken cancellationToken = default)
        {
            var inputSearchTerm = searchTerm.ToLaEstabCodeFormat();
            return await SearchLocalAuthEstablishmentCommon(inputSearchTerm, skip, take, cancellationToken);
        }

        public async Task<Result<SearchSuggestionsResult<EstablishmentSearchSuggestionsResult>>>
            EstablishmentSearchSuggestions(
                string searchTerm, int maxSuggestions, CancellationToken cancellationToken = default)
        {
            return await _documentDB.QueryAsync<SearchSuggestionsResultDAO>(ContainerKey,
                    q => q.Where(x =>
                            !x.IsDeleted && x.IsVisible &&
                            (
                                x.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                (x.Address != null && (
                                    x.Address.Street.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.Address.Town.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.Address.PostCode.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                )) ||
                                x.Urn.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                (x.Laestab != null && (
                                    x.Laestab.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.Laestab.Replace("/", "").Contains(searchTerm,
                                        StringComparison.CurrentCultureIgnoreCase)
                                ))
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
        
        private async Task<Result<SearchResult<EstablishmentDetailsSearchResult>>> SearchLocalAuthEstablishmentCommon(
            string searchTerm, int skip, int take,
            CancellationToken cancellationToken = default)
        {
            return await _documentDB.QueryAsyncPaged<SearchResultDAO>(ContainerKey,
                    q => q.Where(x =>
                            !x.IsDeleted && x.IsVisible &&
                            x.Laestab != null && // Ensure Laestab is not null before calling Contains
                            x.Laestab.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))
                        .OrderBy(x => x.Name), skip, take, cancellationToken)
                .ErrorIf(q => q.TotalCount == 0, Error.NotFound($@"there were no matches for ""{searchTerm}""."))
                .Map(x => new SearchResult<EstablishmentDetailsSearchResult>
                {
                    Results = x.Items.MapToEstablishmentDetailsSearchResults(),
                    SearchTerm = searchTerm,
                    TotalResults = x.TotalCount,
                    ResultsPerPage = take
                });
        }
    }
}