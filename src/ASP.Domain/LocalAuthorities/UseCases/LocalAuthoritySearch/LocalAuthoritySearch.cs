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

    public Task<Result<ResultsPage<LocalAuthority>>> HandleRequest(
        LocalAuthoritySearchRequest request)
    {
        var page = request.Page.GetValueOrDefault(1);
        var resultsPerPage = request.ResultsPerPage.GetValueOrDefault(Core.Constants.SearchResultPageSize);
        var isNumeric = int.TryParse(request.SearchTerm, out var _);

        var localAuthoritySearchResults = isNumeric
            ? from laCode in LACode.Parse(request.SearchTerm)
                .MapErrorIf(e => e is ValidationError, Error.NotFound($@"there were no matches for ""{request.SearchTerm}""."))
              from localAuthority in _repository.Get(laCode)
                .MapErrorIf(e => e is NotFoundError, Error.NotFound($@"there were no matches for ""{request.SearchTerm}""."))
              select ResultsPage.SingleItem(page, resultsPerPage, localAuthority)
            : _repository.Search(new NameSearchCriteria(request.SearchTerm), page, resultsPerPage);
        
        return localAuthoritySearchResults;
    }
}