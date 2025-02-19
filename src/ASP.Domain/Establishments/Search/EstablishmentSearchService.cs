using ASP.Core.Pagination;
using ASP.Core.Results;

namespace ASP.Domain.Establishments.Search
{
    public class EstablishmentSearchService
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly EstablishmentSearchStrategyFactory _strategyFactory;

        public EstablishmentSearchService(
            IEstablishmentRepository establishmentRepository
        )
        {
            _establishmentRepository = establishmentRepository;

            _strategyFactory = new EstablishmentSearchStrategyFactory(_establishmentRepository);
        }

        public async Task<Result<ResultsPage<EstablishmentListing>>> Search(string searchTerm, EstablishmentScope scope, int page, int resultsPerPage)
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

            return await initialStrategy.Execute()
                .IfErrorThen(
                    e => e is NotFoundError && searchType != SearchType.EstablishmentNameOrLocation,
                    backupStrategy.Execute
                );
        }
    }
}