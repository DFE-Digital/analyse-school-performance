using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools.Search
{
    public class SearchSuggestionsService
    {
        private readonly ISchoolRepository _repository;

        public SearchSuggestionsService(ISchoolRepository repository)
        {
            _repository = repository;
        }

        public Task<Result<List<School>>> Search(string searchTerm, Optional<SchoolAccessScope> scope, int maxSuggestions)
        {
            var isNumeric = int.TryParse(searchTerm, out var _);

            var suggestions = isNumeric
              ? from urnResults in _repository.SearchSuggestionsByCriteria(new PartialSchoolUrnSearchCriteria(searchTerm), scope, maxSuggestions)
                    .DefaultIf(error => error is NotFoundError, [])
                from laEstabResults in _repository.SearchSuggestionsByCriteria(new PartialLAEstabCodeSearchCriteria(searchTerm), scope, maxSuggestions)
                    .DefaultIf(error => error is NotFoundError, [])
                from nameAddressResults in _repository.SearchSuggestionsByCriteria(new NameOrAddressSearchCriteria(searchTerm), scope, maxSuggestions)
                    .DefaultIf(error => error is NotFoundError, [])
                let combinedResults = urnResults
                    .Concat(laEstabResults)
                    .Concat(nameAddressResults)
                    .DistinctBy(r => r.Urn)
                    .Take(maxSuggestions)
                    .ToList()
                from results in combinedResults.Any()
                  ? Result.Success(combinedResults)
                  : Error.NotFound($@"There were no matches for ""{searchTerm}"" within the given scope.")
                select results
              : _repository.SearchSuggestionsByCriteria(new PartialLaEstabCodeOrNameOrAddressSearchCriteria(searchTerm), scope, maxSuggestions);

            return suggestions;
        }
    }
}
