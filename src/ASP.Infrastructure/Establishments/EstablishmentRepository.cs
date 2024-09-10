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
            return _documentDB.GetAsync<EstablishmentDAO>(ContainerKey, urn, urn)
                .MapError(e => e is NotFoundError
                    ? Error.NotFound($@"Could not find Establishment with URN ""{urn}"".")
                    : e)
                .ErrorIf(estab => estab.IsDeleted, Error.NotFound($@"Establishment with URN ""{urn}"" has been deleted."))
                .ErrorIf(estab => !estab.IsVisible, Error.NotFound($@"Establishment with URN ""{urn}"" is not currently visible."))
                .Map(dto => dto.MapToEstablishmentDetails());
        }

        public Task<Result<Done>> Create(string contentId, EstablishmentDetails establishmentDetails)
        {
            return _documentDB.UpsertAsync(ContainerKey, contentId, establishmentDetails.Urn,
                establishmentDetails);
        }

        public async Task<Result<SearchResultsPage<EstablishmentListItem>>> SearchEstablishmentNameOrLocation(
            Scope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            return await _documentDB.QueryPagedAsync<EstablishmentDAO>(
                    ContainerKey,
                    ApplyScopeAndSearchQuery(scope, searchTerm),
                    page,
                    resultsPerPage,
                    cancellationToken)
                .ErrorIf(q => q.TotalResults == 0, Error.NotFound($@"there were no matches for ""{searchTerm}"" within the given scope."))
                .Map(results => new SearchResultsPage<EstablishmentListItem>(searchTerm, scope.ScopeType.ToString(),
                    scope.ScopeIdentifier, results.Map(r => r.MapToEstablishmentListItem())));
        }

        public async Task<Result<SearchResultsPage<EstablishmentListItem>>> SearchEstablishmentByLaCode(
            Scope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            var inputSearchTerm = searchTerm + "/";

            return await SearchByLaestabCommon(scope, inputSearchTerm, searchTerm, page, resultsPerPage, cancellationToken);
        }

        public async Task<Result<SearchResultsPage<EstablishmentListItem>>> SearchEstablishmentByEstablishmentNumber(
            Scope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            var inputSearchTerm = "/" + searchTerm;

            return await SearchByLaestabCommon(scope, inputSearchTerm, searchTerm, page, resultsPerPage, cancellationToken);
        }

        public async Task<Result<SearchResultsPage<EstablishmentListItem>>> SearchEstablishmentByLaCodeOrEstablishmentNumber(
            Scope scope,
            string searchTerm,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default
        )
        {
            return await SearchByLaestabCommon(scope, searchTerm, searchTerm, page, resultsPerPage, cancellationToken);
        }

        public async Task<Result<SearchResultsPage<EstablishmentListItem>>> SearchEstablishmentByLaestab7DigitCode(
            Scope scope,
            string searchTerm,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default
        )
        {
            var inputSearchTerm = searchTerm.ToLaEstabCodeFormat();

            return await SearchByLaestabCommon(scope, inputSearchTerm, searchTerm, page, resultsPerPage,
                cancellationToken);
        }

        public async Task<Result<List<EstablishmentSearchSuggestionsResult>>> GetEstablishmentSearchSuggestions(Scope scope, string searchTerm, int maxSuggestions, CancellationToken cancellationToken = default)
        {
            return await _documentDB.QueryAsync(
                ContainerKey,
                ApplyScopeAndSearchSuggestionsQuery(scope, searchTerm, maxSuggestions),
                cancellationToken
            )
            .Map(r => r.MapToEstablishmentSearchSuggestionsResults())
            .ErrorIf(r => r.Count == 0, Error.NotFound($@"there were no matches for ""{searchTerm}"" within the given scope."));
        }

        public async Task<Result<List<EstablishmentSearchSuggestionsResult>>> GetEstablishmentSearchSuggestionsByUrn(Scope scope, string searchTerm, int maxSuggestions, CancellationToken cancellationToken = default)
        {
            return await _documentDB.QueryAsync(
                ContainerKey,
                ApplyScopeAndSearchSuggestionsUrnQuery(scope, searchTerm, maxSuggestions),
                cancellationToken
            )
            .Map(r => r.MapToEstablishmentSearchSuggestionsResults())
            .ErrorIf(r => r.Count == 0, Error.NotFound($@"there were no matches for ""{searchTerm}"" within the given scope."));
        }

        public async Task<Result<List<EstablishmentSearchSuggestionsResult>>> GetEstablishmentSearchSuggestionsByLaEstab(Scope scope, string searchTerm, int maxSuggestions, CancellationToken cancellationToken = default)
        {
            return await _documentDB.QueryAsync(
                ContainerKey,
                ApplyScopeAndSearchSuggestionsLaestabQuery(scope, searchTerm, maxSuggestions),
                cancellationToken
            )
            .Map(r => r.MapToEstablishmentSearchSuggestionsResults())
            .ErrorIf(r => r.Count == 0, Error.NotFound($@"there were no matches for ""{searchTerm}"" within the given scope."));
        }

        public async Task<Result<List<EstablishmentSearchSuggestionsResult>>> GetEstablishmentSearchSuggestionsByNameAddress(Scope scope, string searchTerm, int maxSuggestions, CancellationToken cancellationToken = default)
        {
            return await _documentDB.QueryAsync(
                ContainerKey,
                ApplyScopeAndSearchSuggestionsNameAddressQuery(scope, searchTerm, maxSuggestions),
                cancellationToken
            )
            .Map(r => r.MapToEstablishmentSearchSuggestionsResults())
            .ErrorIf(r => r.Count == 0, Error.NotFound($@"there were no matches for ""{searchTerm}"" within the given scope."));
        }

        private async Task<Result<SearchResultsPage<EstablishmentListItem>>> SearchByLaestabCommon(
            Scope scope,
            string searchTerm,
            string originalSearchTerm,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default
        )
        {
            return await _documentDB.QueryPagedAsync<EstablishmentDAO>(
                    ContainerKey,
                    ApplyScopeAndSearchQuery(scope, searchTerm, true),
                    page,
                    resultsPerPage,
                    cancellationToken)
                .ErrorIf(q => q.TotalResults == 0, Error.NotFound($@"there were no matches for ""{originalSearchTerm}""."))
                .Map(results => new SearchResultsPage<EstablishmentListItem>(originalSearchTerm, scope.ScopeType.ToString(),
                    scope.ScopeIdentifier, results.Map(r => r.MapToEstablishmentListItem())));
        }

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> ScopeWhere(Scope scope) =>
            scope.ScopeType switch
            {
                ScopeType.All => q => q,
                ScopeType.LA => q => q.Where(x =>
                    x.LocalAuthority != null && (x.LocalAuthority.Code.ToString() == scope.ScopeIdentifier)),
                ScopeType.MAT => q => q.Where(x =>
                    x.MultiAcademyTrust != null && (x.MultiAcademyTrust.Uid.ToString() == scope.ScopeIdentifier)),
                ScopeType.Diocese => q => q.Where(x => x.Diocese != null && (x.Diocese.Name == scope.ScopeIdentifier)),
                _ => throw new ArgumentOutOfRangeException(nameof(scope))
            };

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> SearchEstablishmentNameOrLocationWhere(
            string searchTerm) =>
            q => q.Where(x =>
                !x.IsDeleted && x.IsVisible && (
                    x.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                    x.Address != null && (
                        x.Address.Street.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                        x.Address.Town.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                        x.Address.PostCode.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                    )));
        
        
        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> SearchByLaestabCommonWhere(
            string searchTerm) =>
            q => q.Where(x =>
                !x.IsDeleted && x.IsVisible &&
                x.Laestab != null && // Ensure Laestab is not null before calling Contains
                x.Laestab.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
            );

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> EstablishmentSearchSuggestionsWhere(
            string searchTerm) =>
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
                ));

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> ApplyScopeAndSearchQuery(Scope scope,
            string searchTerm, bool isLocalAuthEstablishmentCommon = false)
        {
            return queryable =>
            {
                // Apply scope filter
                var scopedQuery = ScopeWhere(scope)(queryable);
                
                // apply other filter
                scopedQuery = isLocalAuthEstablishmentCommon
                    ? SearchByLaestabCommonWhere(searchTerm)(scopedQuery)
                    : SearchEstablishmentNameOrLocationWhere(searchTerm)(scopedQuery);

                return scopedQuery.OrderBy(x => x.Name);
            };
        }
        
        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>>
            ApplyScopeAndSearchSuggestionsQuery(Scope scope,
            string searchTerm, int maxSuggestions)
        {
            return queryable =>
            {
                // Apply scope filter
                var scopedQuery = ScopeWhere(scope)(queryable);
                
                // apply other filter
                scopedQuery = EstablishmentSearchSuggestionsWhere(searchTerm)(scopedQuery);

                return scopedQuery.OrderBy(x => x.Name)
                    .Take(maxSuggestions);
            };
        }

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>>
            ApplyScopeAndSearchSuggestionsUrnQuery(Scope scope,
                string searchTerm, int maxSuggestions)
        {
            return queryable =>
            {
                // Apply scope filter
                var scopedQuery = ScopeWhere(scope)(queryable);
                var urnMatches = scopedQuery
                    .Where(x => !x.IsDeleted && x.IsVisible && (x.Urn.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)));

                return urnMatches.OrderBy(x => x.Urn).Take(maxSuggestions);
            };
        }

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>>
            ApplyScopeAndSearchSuggestionsLaestabQuery(Scope scope,
                string searchTerm, int maxSuggestions)
        {
            return queryable =>
            {
                // Apply scope filter
                var scopedQuery = ScopeWhere(scope)(queryable);
                var laestabMatches = scopedQuery
                    .Where(x => !x.IsDeleted && x.IsVisible &&
                                (x.Laestab != null &&
                                (x.Laestab.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) || x.Laestab
                                    .Replace("/", "").Contains(searchTerm,
                                        StringComparison.CurrentCultureIgnoreCase))));

                return laestabMatches.OrderBy(x => x.Laestab).Take(maxSuggestions);
            };
        }

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>>
            ApplyScopeAndSearchSuggestionsNameAddressQuery(Scope scope,
                string searchTerm, int maxSuggestions)
        {
            return queryable =>
            {
                // Apply scope filter
                var scopedQuery = ScopeWhere(scope)(queryable);
                var nameAddressMatches = scopedQuery
                    .Where(x => !x.IsDeleted && x.IsVisible &&
                                (x.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                (x.Address != null &&
                                 (x.Address.Street.Contains(searchTerm,
                                      StringComparison.CurrentCultureIgnoreCase) ||
                                  x.Address.Town.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                  x.Address.PostCode.Contains(searchTerm,
                                      StringComparison.CurrentCultureIgnoreCase)))));

                return nameAddressMatches.OrderBy(x => x.Name).Take(maxSuggestions);
            };
        }
    }
}