using ASP.Domain.Establishments;
using ASP.Domain.Establishments.SearchSuggestions;
using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Infrastructure.DocumentDatabase;
using ASP.Domain.Repositories.Establishments.DAO;
using ASP.Domain.Repositories.Establishments.DAO.Mapper;

namespace ASP.Domain.Repositories.Establishments
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
            return
                from dao in _documentDB.GetAsync<EstablishmentDAO>(ContainerKey, urn, urn)
                    .MapError(e => e is NotFoundError
                        ? Error.NotFound($@"Could not find Establishment with URN ""{urn}"".")
                        : e)
                    .ErrorIf(estab => estab.IsDeleted, Error.NotFound($@"Establishment with URN ""{urn}"" has been deleted."))
                    .ErrorIf(estab => !estab.IsVisible, Error.NotFound($@"Establishment with URN ""{urn}"" is not currently visible."))
                select dao.MapToEstablishmentDetails();
        }
        
        public Task<Result<bool>> IsEstablishmentVisibleWithinScope(string urn, EstablishmentScope scope, CancellationToken cancellationToken = default)
        {
            return _documentDB.GetAsync<EstablishmentDAO>(ContainerKey, urn, urn, cancellationToken)
                .MapError(e => e is NotFoundError
                    ? Error.NotFound($@"School with URN ""{urn}"" does not exist.")
                    : e)
                .Then(result => 
                {
                    bool isInScope = scope.ScopeType switch
                    {
                        EstablishmentScopeType.All => true,
                        EstablishmentScopeType.LA => result.LocalAuthority != null && 
                                        result.LocalAuthority.Code.ToString() == scope.ScopeIdentifier,
                        EstablishmentScopeType.MAT => result.MultiAcademyTrust != null && 
                                         result.MultiAcademyTrust.Uid.ToString() == scope.ScopeIdentifier,
                        EstablishmentScopeType.Diocese => result.Diocese != null && 
                                             result.Diocese.Name == scope.ScopeIdentifier,
                        _ => false // Default case returns false for any unhandled ScopeType
                    };

                    return Result.Success(isInScope);
                });
        }
        
        public Task<Result<List<EstablishmentDetails>>> GetLinkedEstablishments(
            string urn, CancellationToken cancellationToken = default)
        {
            return _documentDB.QueryAsync(
                    ContainerKey,
                    GetLinkedEstablishmentsQuery(urn),
                    cancellationToken)
                .MapError(e => e is NotFoundError
                    ? Error.NotFound($@"School with URN ""{urn}"" does not exist.")
                    : e)
                .Map(linkedEstablishments =>
                {
                    return linkedEstablishments.Select(e => e.MapToEstablishmentDetails()).ToList();
                });
        }
        
        public Task<Result<Done>> Create(string contentId, EstablishmentDetails establishmentDetails)
        {
            return _documentDB.UpsertAsync(ContainerKey, contentId, establishmentDetails.Urn, establishmentDetails);
        }

        public Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentNameOrLocation(
            EstablishmentScope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            return
                from results in _documentDB.QueryPagedAsync(
                    ContainerKey,
                    ApplyScopeAndSearchQuery(scope, searchTerm),
                    page,
                    resultsPerPage,
                    cancellationToken)
                    .ErrorIf(q => q.TotalResults == 0, Error.NotFound($@"there were no matches for ""{searchTerm}"" within the given scope."))
                select new ScopedSearchResultsPage<EstablishmentListing>(
                    searchTerm,
                    scope.ScopeType.ToString(),
                    scope.ScopeIdentifier,
                    results.Map(r => r.MapToEstablishmentListing()));
        }

        public Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentByLaCode(
            EstablishmentScope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            var inputSearchTerm = searchTerm + "/";

            return SearchByLaestabCommon(scope, inputSearchTerm, searchTerm, page, resultsPerPage, cancellationToken);
        }

        public Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentByEstablishmentNumber(
            EstablishmentScope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            var inputSearchTerm = "/" + searchTerm;

            return SearchByLaestabCommon(scope, inputSearchTerm, searchTerm, page, resultsPerPage, cancellationToken);
        }

        public Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentByLaCodeOrEstablishmentNumber(
            EstablishmentScope scope,
            string searchTerm,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            return SearchByLaestabCommon(scope, searchTerm, searchTerm, page, resultsPerPage, cancellationToken);
        }

        public Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentByLaestab7DigitCode(
            EstablishmentScope scope,
            string searchTerm,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            var inputSearchTerm = searchTerm.ToLaEstabCodeFormat();

            return SearchByLaestabCommon(scope, inputSearchTerm, searchTerm, page, resultsPerPage, cancellationToken);
        }

        public Task<Result<List<EstablishmentSuggestion>>> GetEstablishmentSearchSuggestions(
            EstablishmentScope scope,
            string searchTerm,
            int maxSuggestions,
            CancellationToken cancellationToken = default)
        {
            return
                from results in _documentDB.QueryAsync(
                    ContainerKey,
                    ApplyScopeAndSearchSuggestionsQuery(scope, searchTerm, maxSuggestions),
                    cancellationToken)
                    .ErrorIf(r => !r.Any(), Error.NotFound($@"there were no matches for ""{searchTerm}"" within the given scope."))
                select results.MapToEstablishmentSuggestions();
        }

        public Task<Result<List<EstablishmentSuggestion>>> GetEstablishmentSearchSuggestionsByUrn(
            EstablishmentScope scope,
            string searchTerm,
            int maxSuggestions,
            CancellationToken cancellationToken = default)
        {
            return
                from results in _documentDB.QueryAsync(
                    ContainerKey,
                    ApplyScopeAndSearchSuggestionsUrnQuery(scope, searchTerm, maxSuggestions),
                    cancellationToken)
                   .ErrorIf(r => !r.Any(), Error.NotFound($@"there were no matches for ""{searchTerm}"" within the given scope."))
                select results.MapToEstablishmentSuggestions();
        }

        public Task<Result<List<EstablishmentSuggestion>>> GetEstablishmentSearchSuggestionsByLaEstab(
            EstablishmentScope scope,
            string searchTerm,
            int maxSuggestions,
            CancellationToken cancellationToken = default)
        {
            return
                from results in _documentDB.QueryAsync(
                    ContainerKey,
                    ApplyScopeAndSearchSuggestionsLaestabQuery(scope, searchTerm, maxSuggestions),
                    cancellationToken)
                    .ErrorIf(r => !r.Any(), Error.NotFound($@"there were no matches for ""{searchTerm}"" within the given scope."))
                select results.MapToEstablishmentSuggestions();
        }

        public Task<Result<List<EstablishmentSuggestion>>> GetEstablishmentSearchSuggestionsByNameAddress(
            EstablishmentScope scope,
            string searchTerm,
            int maxSuggestions,
            CancellationToken cancellationToken = default)
        {
            return
                from results in _documentDB.QueryAsync(
                    ContainerKey,
                    ApplyScopeAndSearchSuggestionsNameAddressQuery(scope, searchTerm, maxSuggestions),
                    cancellationToken)
                    .ErrorIf(r => !r.Any(), Error.NotFound($@"there were no matches for ""{searchTerm}"" within the given scope."))
                select results.MapToEstablishmentSuggestions();
        }

        public Task<Result<ScopedResultsPage<EstablishmentListing>>> GetAllEstablishments(
            EstablishmentScope scope,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            return
                from results in _documentDB.QueryPagedAsync(
                    ContainerKey,
                    EstablishmentVisibleAndNotDeleteWithScopeWhere(scope),
                    page,
                    resultsPerPage,
                    cancellationToken)
                    .ErrorIf(q => q.TotalResults == 0, Error.NotFound("there were no establishments within the given scope."))
                select new ScopedResultsPage<EstablishmentListing>(
                    scope.ScopeType.ToString(),
                    scope.ScopeIdentifier,
                    results.Map(r => r.MapToEstablishmentListing()));
        }

        private Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchByLaestabCommon(
            EstablishmentScope scope,
            string searchTerm,
            string originalSearchTerm,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default
        )
        {
            return
                from results in _documentDB.QueryPagedAsync(
                    ContainerKey,
                    ApplyScopeAndSearchQuery(scope, searchTerm, true),
                    page,
                    resultsPerPage,
                    cancellationToken)
                    .ErrorIf(q => q.TotalResults == 0, Error.NotFound($@"there were no matches for ""{originalSearchTerm}""."))
                select new ScopedSearchResultsPage<EstablishmentListing>(
                    originalSearchTerm,
                    scope.ScopeType.ToString(),
                    scope.ScopeIdentifier,
                    results.Map(r => r.MapToEstablishmentListing()));
        }

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> ScopeWhere(EstablishmentScope scope) =>
            scope.ScopeType switch {
                EstablishmentScopeType.All => q => q,

                EstablishmentScopeType.LA => q => q.Where(x => x.LocalAuthority != null
                                        && x.LocalAuthority.Code.ToString() == scope.ScopeIdentifier),

                EstablishmentScopeType.MAT => q => q.Where(x => x.MultiAcademyTrust != null
                                        && x.MultiAcademyTrust.Uid.ToString() == scope.ScopeIdentifier),

                EstablishmentScopeType.Diocese => q => q.Where(x => x.Diocese != null
                                        && x.Diocese.Name == scope.ScopeIdentifier),

                _ => throw new ArgumentOutOfRangeException(nameof(scope))
            };

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> EstablishmentVisibleAndNotDeletedWhere() =>
            q => q.Where(x => !x.IsDeleted && x.IsVisible);

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> EstablishmentVisibleAndNotDeleteWithScopeWhere(EstablishmentScope scope)
        {
            return queryable =>
            {
                // Apply scope filter
                var scopedQuery = ScopeWhere(scope)(queryable);

                // Apply visible and not deleted filter
                var visibleAndNotDeletedQuery = EstablishmentVisibleAndNotDeletedWhere()(scopedQuery);

                // Apply ordering
                return visibleAndNotDeletedQuery
                    .OrderBy(x => x.Name);
            };
        }

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> SearchEstablishmentNameOrLocationWhere(string searchTerm) =>
            q => q.Where(x => !x.IsDeleted && x.IsVisible && (
                    x.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                    x.Address != null && (
                        x.Address.Street.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                        x.Address.Town.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                        x.Address.PostCode.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                    )));


        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> SearchByLaestabCommonWhere(string searchTerm) =>
            q => q.Where(x => !x.IsDeleted && x.IsVisible &&
                    x.Laestab != null && // Ensure Laestab is not null before calling Contains
                    x.Laestab.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
            );

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> EstablishmentSearchSuggestionsWhere(string searchTerm) =>
            q => q.Where(x => !x.IsDeleted && x.IsVisible && (
                    x.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                    x.Address != null && (
                        x.Address.Street.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                        x.Address.Town.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                        x.Address.PostCode.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                    ) ||
                    x.Urn.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                    x.Laestab != null && (
                        x.Laestab.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                        x.Laestab.Replace("/", "").Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                    )));

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> ApplyScopeAndSearchQuery(
            EstablishmentScope scope,
            string searchTerm,
            bool isLocalAuthEstablishmentCommon = false)
        {
            return queryable =>
            {
                // Apply scope filter
                var scopedQuery = ScopeWhere(scope)(queryable);

                // apply other filter
                scopedQuery = isLocalAuthEstablishmentCommon
                    ? SearchByLaestabCommonWhere(searchTerm)(scopedQuery)
                    : SearchEstablishmentNameOrLocationWhere(searchTerm)(scopedQuery);

                return scopedQuery
                    .OrderBy(x => x.Name);
            };
        }

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> ApplyScopeAndSearchSuggestionsQuery(
            EstablishmentScope scope,
            string searchTerm,
            int maxSuggestions)
        {
            return queryable =>
            {
                // Apply scope filter
                var scopedQuery = ScopeWhere(scope)(queryable);

                // apply other filter
                scopedQuery = EstablishmentSearchSuggestionsWhere(searchTerm)(scopedQuery);

                return scopedQuery
                    .OrderBy(x => x.Name)
                    .Take(maxSuggestions);
            };
        }

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> ApplyScopeAndSearchSuggestionsUrnQuery(
            EstablishmentScope scope,
            string searchTerm,
            int maxSuggestions)
        {
            return queryable =>
            {
                // Apply scope filter
                var scopedQuery = ScopeWhere(scope)(queryable);

                var urnMatches = scopedQuery
                    .Where(x => !x.IsDeleted && x.IsVisible &&
                        x.Urn.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase));

                return urnMatches
                    .OrderBy(x => x.Urn)
                    .Take(maxSuggestions);
            };
        }
        
        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> ApplyScopeAndSearchSuggestionsLaestabQuery(
            EstablishmentScope scope,
            string searchTerm,
            int maxSuggestions)
        {
            return queryable =>
            {
                // Apply scope filter
                var scopedQuery = ScopeWhere(scope)(queryable);

                var laestabMatches = scopedQuery
                    .Where(x => !x.IsDeleted && x.IsVisible &&
                        x.Laestab != null && (
                            x.Laestab.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                            x.Laestab.Replace("/", "").Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)));

                return laestabMatches
                    .OrderBy(x => x.Laestab)
                    .Take(maxSuggestions);
            };
        }

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> ApplyScopeAndSearchSuggestionsNameAddressQuery(
            EstablishmentScope scope,
            string searchTerm,
            int maxSuggestions)
        {
            return queryable =>
            {
                // Apply scope filter
                var scopedQuery = ScopeWhere(scope)(queryable);

                var nameAddressMatches = scopedQuery
                    .Where(x => !x.IsDeleted && x.IsVisible && (
                        x.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                            x.Address != null && (
                                x.Address.Street.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                x.Address.Town.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                x.Address.PostCode.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))));

                return nameAddressMatches
                    .OrderBy(x => x.Name)
                    .Take(maxSuggestions);
            };
        }
        
        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> GetLinkedEstablishmentsQuery(string urn)
        {
            return queryable =>
            {
                // First, get the main establishment's links
                var mainEstablishmentLinks = queryable
                    .Where(e => e.Urn == urn && !e.IsDeleted && e.IsVisible)
                    .SelectMany(e => e.Links ?? Enumerable.Empty<LinkDAO>())
                    .Select(l => l.LinkedUrn);

                // Then get all the linked establishments
                return queryable
                    .Where(e => mainEstablishmentLinks.Contains(e.Urn) && !e.IsDeleted && e.IsVisible)
                    .OrderBy(e => e.Name);
            };
        }
    }
}