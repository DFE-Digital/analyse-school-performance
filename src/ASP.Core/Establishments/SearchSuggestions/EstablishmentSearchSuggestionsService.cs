using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Results;

namespace ASP.Core.Establishments.SearchSuggestions
{
    public class EstablishmentSearchSuggestionsService
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly ILocalAuthorityRepository _localAuthorityRepository;
        private readonly IMultiAcademyTrustRepository _multiAcademyTrustRepository;

        public EstablishmentSearchSuggestionsService(
            IEstablishmentRepository establishmentRepository,
            ILocalAuthorityRepository localAuthorityRepository,
            IMultiAcademyTrustRepository multiAcademyTrustRepository
        )
        {
            _establishmentRepository = establishmentRepository;
            _localAuthorityRepository = localAuthorityRepository;
            _multiAcademyTrustRepository = multiAcademyTrustRepository;
        }

        public async Task<Result<SearchSuggestionsResult<EstablishmentSuggestion>>> Search(string searchTerm, Scope.Scope scope, int maxSuggestions)
        {
            return await scope.Validate(_localAuthorityRepository, _multiAcademyTrustRepository)
                .Then(async t =>
                {
                    var isNumeric = int.TryParse(searchTerm, out _);

                    if (isNumeric)
                    {
                        return await _establishmentRepository.GetEstablishmentSearchSuggestionsByUrn(scope, searchTerm, maxSuggestions)
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
                                    })));
                    }

                    return await _establishmentRepository.GetEstablishmentSearchSuggestions(scope, searchTerm, maxSuggestions);
                })
                .Map(results => new SearchSuggestionsResult<EstablishmentSuggestion> {
                    Suggestions = results,
                    SearchTerm = searchTerm,
                    MaxSuggestions = maxSuggestions
                });
        }
    }
}
