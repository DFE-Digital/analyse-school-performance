using ASP.Core.Results;

namespace ASP.Domain.Establishments.SearchSuggestions
{
    public class EstablishmentSearchSuggestionsService
    {
        private readonly IEstablishmentRepository _establishmentRepository;

        public EstablishmentSearchSuggestionsService(
            IEstablishmentRepository establishmentRepository
        )
        {
            _establishmentRepository = establishmentRepository;
        }

        public Task<Result<SearchSuggestionsResult<EstablishmentSuggestion>>> Search(string searchTerm, EstablishmentScope scope, int maxSuggestions)
        {
            var isNumeric = int.TryParse(searchTerm, out var _);

            var suggestions = isNumeric
              ? from urnResults in _establishmentRepository.GetEstablishmentSearchSuggestionsByUrn(scope, searchTerm, maxSuggestions)
                    .DefaultIf(error => error is NotFoundError, [])
                from laEstabResults in _establishmentRepository.GetEstablishmentSearchSuggestionsByLaEstab(scope, searchTerm, maxSuggestions)
                    .DefaultIf(error => error is NotFoundError, [])
                from nameAddressResults in _establishmentRepository.GetEstablishmentSearchSuggestionsByNameAddress(scope, searchTerm, maxSuggestions)
                    .DefaultIf(error => error is NotFoundError, [])
                let combinedResults = urnResults
                    .Concat(laEstabResults)
                    .Concat(nameAddressResults)
                    .Distinct()
                    .Take(maxSuggestions)
                    .ToList()
                from results in combinedResults.Any()
                  ? Result.Success(combinedResults)
                  : Error.NotFound($@"there were no matches for ""{searchTerm}"" within the given scope.")
                select results
              : _establishmentRepository.GetEstablishmentSearchSuggestions(scope, searchTerm, maxSuggestions);
                
            return suggestions
                .Map(results => new SearchSuggestionsResult<EstablishmentSuggestion> {
                    Suggestions = results,
                    SearchTerm = searchTerm,
                    MaxSuggestions = maxSuggestions
                });
        }
    }
}
