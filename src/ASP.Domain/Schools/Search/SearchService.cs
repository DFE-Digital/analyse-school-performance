using ASP.Core.Optionality;
using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools.Search
{
    public class SearchService
    {
        private readonly ISchoolRepository _repository;
        private readonly SearchStrategyFactory _strategyFactory;

        public SearchService(ISchoolRepository repository)
        {
            _repository = repository;

            _strategyFactory = new SearchStrategyFactory(_repository);
        }

        public async Task<Result<ResultsPage<School>>> Search(
            string searchTerm,
            Optional<SchoolAccessScope> scope,
            int page,
            int resultsPerPage)
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
                SearchType.NameOrLocation,
                searchTerm,
                page,
                resultsPerPage
            );

            return await initialStrategy.Execute()
                .IfErrorThen(
                    e => e is NotFoundError && searchType != SearchType.NameOrLocation,
                    backupStrategy.Execute
                );
        }
    }
}