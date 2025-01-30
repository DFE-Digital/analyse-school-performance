using ASP.Core;
using ASP.Core.Establishments;
using ASP.Core.Establishments.Search;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Extensions;
using ASP.Core.Results;
using ASP.Core.Scoping;
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
            return 
                from dao in _documentDB.GetAsync<EstablishmentDAO>(ContainerKey, urn, urn)
                    .MapError(e => e is NotFoundError
                        ? Error.NotFound($@"Could not find Establishment with URN ""{urn}"".")
                        : e)
                    .ErrorIf(estab => estab.IsDeleted, Error.NotFound($@"Establishment with URN ""{urn}"" has been deleted."))
                    .ErrorIf(estab => !estab.IsVisible, Error.NotFound($@"Establishment with URN ""{urn}"" is not currently visible."))
                select dao.MapToEstablishmentDetails();
        }
        
        public Task<Result<bool>> IsEstablishmentVisibleWithinScope(string urn, Scope scope, CancellationToken cancellationToken = default)
        {
            return _documentDB.GetAsync<EstablishmentDAO>(ContainerKey, urn, urn, cancellationToken)
                .MapError(e => e is NotFoundError
                    ? Error.NotFound($@"School with URN ""{urn}"" does not exist.")
                    : e)
                .Then(result => 
                {
                    bool isInScope = scope.ScopeType switch
                    {
                        ScopeType.All => true,
                        ScopeType.LA => result.LocalAuthority != null && 
                                        result.LocalAuthority.Code.ToString() == scope.ScopeIdentifier,
                        ScopeType.MAT => result.MultiAcademyTrust != null && 
                                         result.MultiAcademyTrust.Uid.ToString() == scope.ScopeIdentifier,
                        ScopeType.Diocese => result.Diocese != null && 
                                             result.Diocese.Name == scope.ScopeIdentifier,
                        _ => false // Default case returns false for any unhandled ScopeType
                    };

                    return Result.Success(isInScope);
                });
        }
        
        public Task<Result<Done>> Create(string contentId, EstablishmentDetails establishmentDetails)
        {
            return _documentDB.UpsertAsync(ContainerKey, contentId, establishmentDetails.Urn, establishmentDetails);
        }

        public Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentNameOrLocation(
            Scope scope, string searchTerm, int page, int resultsPerPage,
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
            Scope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            var inputSearchTerm = searchTerm + "/";

            return SearchByLaestabCommon(scope, inputSearchTerm, searchTerm, page, resultsPerPage, cancellationToken);
        }

        public Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentByEstablishmentNumber(
            Scope scope, string searchTerm, int page, int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            var inputSearchTerm = "/" + searchTerm;

            return SearchByLaestabCommon(scope, inputSearchTerm, searchTerm, page, resultsPerPage, cancellationToken);
        }

        public Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentByLaCodeOrEstablishmentNumber(
            Scope scope,
            string searchTerm,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            return SearchByLaestabCommon(scope, searchTerm, searchTerm, page, resultsPerPage, cancellationToken);
        }

        public Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> SearchEstablishmentByLaestab7DigitCode(
            Scope scope,
            string searchTerm,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            var inputSearchTerm = searchTerm.ToLaEstabCodeFormat();

            return SearchByLaestabCommon(scope, inputSearchTerm, searchTerm, page, resultsPerPage, cancellationToken);
        }

        public Task<Result<List<EstablishmentSuggestion>>> GetEstablishmentSearchSuggestions(
            Scope scope, 
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
            Scope scope, 
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
            Scope scope, 
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
            Scope scope, 
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
            Scope scope, 
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
            Scope scope,
            string searchTerm,
            string originalSearchTerm,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default
        )
        {
            return 
                from results in _documentDB.QueryPagedAsync<EstablishmentDAO>(
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

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> ScopeWhere(Scope scope) =>
            scope.ScopeType switch
            {
                ScopeType.All => q => q,

                ScopeType.LA => q => q.Where(x => x.LocalAuthority != null 
                                        && (x.LocalAuthority.Code.ToString() == scope.ScopeIdentifier)),

                ScopeType.MAT => q => q.Where(x => x.MultiAcademyTrust != null 
                                        && (x.MultiAcademyTrust.Uid.ToString() == scope.ScopeIdentifier)),

                ScopeType.Diocese => q => q.Where(x => x.Diocese != null 
                                        && (x.Diocese.Name == scope.ScopeIdentifier)),

                _ => throw new ArgumentOutOfRangeException(nameof(scope))
            };
        
        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> EstablishmentVisibleAndNotDeletedWhere() =>
            q => q.Where(x => !x.IsDeleted && x.IsVisible);

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> EstablishmentVisibleAndNotDeleteWithScopeWhere(Scope scope)
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
            Scope scope, 
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
            Scope scope, 
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
            Scope scope,
            string searchTerm, 
            int maxSuggestions)
        {
            return queryable =>
            {
                // Apply scope filter
                var scopedQuery = ScopeWhere(scope)(queryable);

                var urnMatches = scopedQuery
                    .Where(x => !x.IsDeleted && x.IsVisible && (
                        x.Urn.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)));

                return urnMatches
                    .OrderBy(x => x.Urn)
                    .Take(maxSuggestions);
            };
        }
        
        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> ApplyScopeAndSearchSuggestionsLaestabQuery(
            Scope scope,
            string searchTerm, 
            int maxSuggestions)
        {
            return queryable =>
            {
                // Apply scope filter
                var scopedQuery = ScopeWhere(scope)(queryable);

                var laestabMatches = scopedQuery
                    .Where(x => !x.IsDeleted && x.IsVisible && (
                        x.Laestab != null && (
                            x.Laestab.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) || 
                            x.Laestab.Replace("/", "").Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))));

                return laestabMatches
                    .OrderBy(x => x.Laestab)
                    .Take(maxSuggestions);
            };
        }

        private Func<IQueryable<EstablishmentDAO>, IQueryable<EstablishmentDAO>> ApplyScopeAndSearchSuggestionsNameAddressQuery(
            Scope scope,
            string searchTerm, 
            int maxSuggestions)
        {
            return queryable =>
            {
                // Apply scope filter
                var scopedQuery = ScopeWhere(scope)(queryable);

                var nameAddressMatches = scopedQuery
                    .Where(x => !x.IsDeleted && x.IsVisible && (
                        x.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) || (
                            x.Address != null && (
                                x.Address.Street.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                x.Address.Town.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                x.Address.PostCode.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)))));

                return nameAddressMatches
                    .OrderBy(x => x.Name)
                    .Take(maxSuggestions);
            };
        }
    }
}