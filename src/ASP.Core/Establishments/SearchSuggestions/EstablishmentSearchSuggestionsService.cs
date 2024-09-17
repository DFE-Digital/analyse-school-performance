using ASP.Core.Results;
using ASP.Core.Scoping;

namespace ASP.Core.Establishments.SearchSuggestions
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

        public async Task<Result<SearchSuggestionsResult<EstablishmentSuggestion>>> Search(string searchTerm, Scope scope, int maxSuggestions)
        {
            var isNumeric = int.TryParse(searchTerm, out var _);

            var suggestions = isNumeric
                ? _establishmentRepository.GetEstablishmentSearchSuggestionsByUrn(scope, searchTerm, maxSuggestions)
                    .DefaultIf(error => error is NotFoundError, [])
                    .Then(urnResults => _establishmentRepository.GetEstablishmentSearchSuggestionsByLaEstab(scope, searchTerm, maxSuggestions)
                        .DefaultIf(error => error is NotFoundError, [])
                        .Then(laestabResults => _establishmentRepository.GetEstablishmentSearchSuggestionsByNameAddress(scope, searchTerm, maxSuggestions)
                            .DefaultIf(error => error is NotFoundError, [])
                            .Then(nameAddressResults =>
                            {
                                // Merge results manually
                                var combinedResults = urnResults
                                    .Concat(laestabResults)
                                    .Concat(nameAddressResults)
                                    .Distinct()
                                    .Take(maxSuggestions)
                                    .ToList();

                                return combinedResults.Any()
                                    ? Result.Success(combinedResults)
                                    : Error.NotFound($@"there were no matches for ""{searchTerm}"" within the given scope.");
                            })))
                    : _establishmentRepository.GetEstablishmentSearchSuggestions(scope, searchTerm, maxSuggestions);
                
            return await suggestions
                .Map(results => new SearchSuggestionsResult<EstablishmentSuggestion> {
                    Suggestions = results,
                    SearchTerm = searchTerm,
                    MaxSuggestions = maxSuggestions
                });
        }
    }
}
