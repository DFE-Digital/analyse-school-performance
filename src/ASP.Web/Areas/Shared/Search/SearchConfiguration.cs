using ASP.Core;
using ASP.Web.Areas.Shared.Search.Partial.SearchForm;

namespace ASP.Web.Areas.Shared.Search;

// Search configuration class to encapsulate search-related properties  
public class SearchConfiguration
{
    public SearchFormViewModel SearchForm { get; }

    public SearchConfiguration(SearchFormViewModel searchForm)
    {
        SearchForm = searchForm;
    }

    // Factory methods for common configurations  
    public static SearchConfiguration ForSchools(string searchSuggestionUrl) =>
        new(new SearchFormViewModel(searchSuggestionUrl,
            Constants.AlpineComponentSchoolSearchSuggestions,
            Constants.SchoolSearchFormSearchTermInputLabel,
            Constants.SchoolSearchTermInputValidationMessage)
        );

    public static SearchConfiguration ForLocalAuthorities(string searchSuggestionUrl) =>
        new(new SearchFormViewModel(searchSuggestionUrl,
            Constants.AlpineComponentLaSearchSuggestions,
            Constants.LaSearchFormSearchTermInputLabel,
            Constants.LaSearchTermInputValidationMessage)
        );
}