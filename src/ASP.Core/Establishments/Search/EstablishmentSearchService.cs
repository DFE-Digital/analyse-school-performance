using ASP.Core.Extensions;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Results;

namespace ASP.Core.Establishments.Search
{
    public class EstablishmentSearchService
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly ILocalAuthorityRepository _localAuthorityRepository;
        private readonly IMultiAcademyTrustRepository _multiAcademyTrustRepository;
        private readonly EstablishmentSearchStrategyFactory _strategyFactory;

        public EstablishmentSearchService(
            IEstablishmentRepository establishmentRepository,
            ILocalAuthorityRepository localAuthorityRepository,
            IMultiAcademyTrustRepository multiAcademyTrustRepository
        )
        {
            _establishmentRepository = establishmentRepository;
            _localAuthorityRepository = localAuthorityRepository;
            _multiAcademyTrustRepository = multiAcademyTrustRepository;

            _strategyFactory = new EstablishmentSearchStrategyFactory(_establishmentRepository);
        }

        public async Task<Result<SearchResultsPage<EstablishmentListing>>> Search(string searchTerm, Scope.Scope scope, int page, int resultsPerPage)
        {
            var searchType = searchTerm.ClassifySearchType();

            if (searchType == SearchType.Invalid)
            {
                return Error.Invalid($@"The parameter ""{nameof(searchTerm)}"" : ""{searchTerm}"" with type ""{searchType}"" is invalid");
            }

            var initialStrategy = _strategyFactory.CreateStrategy(
                scope,
                searchType,
                searchTerm,
                page,
                resultsPerPage
            );

            // Backup search strategy if the initial strategy fails (e.g. if it's a 3-digit code we'll do an LA
            // lookup but if we don't find a matching LA then we need to do a full search on name/address)
            var backupStrategy = _strategyFactory.CreateStrategy(
                scope,
                SearchType.EstablishmentNameOrLocation,
                searchTerm,
                page,
                resultsPerPage
            );

            return await scope.Validate(_localAuthorityRepository, _multiAcademyTrustRepository)
                .Then(async x => await initialStrategy.Execute())
                .IfErrorThen(
                    e => e is NotFoundError && searchType != SearchType.EstablishmentNameOrLocation,
                    backupStrategy.Execute
                );
        }
    }
}