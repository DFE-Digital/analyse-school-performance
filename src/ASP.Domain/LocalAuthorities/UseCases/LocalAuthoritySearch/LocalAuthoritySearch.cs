using ASP.Core.Pagination;
using ASP.Core.Results;

namespace ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearch;

public class LocalAuthoritySearch : ILocalAuthoritySearch
{
    private readonly ILocalAuthorityRepository _repository;

    public LocalAuthoritySearch(ILocalAuthorityRepository repository)
    {
        _repository = repository ??
                      throw new ArgumentNullException(nameof(repository));
    }

    public Task<Result<SearchResultsPage<LocalAuthority>>> HandleRequest(
        LocalAuthoritySearchRequest request)
    {
        var page = request.Page.GetValueOrDefault(1);
        var resultsPerPage = request.ResultsPerPage.GetValueOrDefault(Core.Constants.SearchResultPageSize);
        var isNumeric = int.TryParse(request.SearchTerm, out var _);

        var localAuthoritySearchResults = isNumeric
            ? from localAuthority in _repository.GetLocalAuthority(request.SearchTerm).MapError(e => e is NotFoundError
                ? Error.NotFound($@"there were no matches for ""{request.SearchTerm}"".")
                : e)
            select new SearchResultsPage<LocalAuthority>(request.SearchTerm, page, resultsPerPage,
                totalResults: 1, [new LocalAuthority(localAuthority.Code, localAuthority.Name)])
            : _repository.LocalAuthoritySearchByLaName(request.SearchTerm, page, resultsPerPage);
        
        return 
            from response in localAuthoritySearchResults
            select response;
    }
}