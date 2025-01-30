using ASP.Web.Shared.Pagination;
using Constants = ASP.Domain.Constants;

namespace ASP.Web.Features.Search;

public class SearchFormViewModel : SearchViewModel
{
    public string SearchTerm { get; }
    public string SearchSuggestionUrl { get; }
    public PaginationViewModel Pagination { get; }
    public string AlpineComponentName { get; }
    public string InputLabel { get; }
    public string InputValidationMessage { get; }
    public string Width { get; set; } = "full";

    public SearchFormViewModel(
        string searchTerm,
        string searchSuggestionUrl,
        PaginationViewModel pagination,
        string alpineComponentName,
        string inputLabel,
        string inputValidationMessage)
    {
        SearchTerm = searchTerm;
        SearchSuggestionUrl = searchSuggestionUrl;
        Pagination = pagination;
        AlpineComponentName = alpineComponentName;
        InputLabel = inputLabel;
        InputValidationMessage = inputValidationMessage;
    }

    // Factory methods for common configurations  
    public static SearchFormViewModel ForSchools(string searchTerm, string searchSuggestionUrl, PaginationViewModel pagination) =>
        new SearchFormViewModel(
            searchTerm,
            searchSuggestionUrl,
            pagination,
            Constants.AlpineComponentSchoolSearchSuggestions,
            Constants.SchoolSearchFormSearchTermInputLabel,
            Constants.SchoolSearchTermInputValidationMessage
        );

    public static SearchFormViewModel ForLocalAuthorities(string searchTerm, string searchSuggestionUrl, PaginationViewModel pagination) =>
        new SearchFormViewModel(
            searchTerm,
            searchSuggestionUrl,
            pagination,
            Constants.AlpineComponentLaSearchSuggestions,
            Constants.LaSearchFormSearchTermInputLabel,
            Constants.LaSearchTermInputValidationMessage
        );
}