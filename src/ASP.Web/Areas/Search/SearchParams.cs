namespace ASP.Web.Areas.Search;

public class SearchParams
{ 
    public string? SearchTerm { get; set; }
    public string? SuggestionSearchTerm { get; set; }
    public int Page { get; set; }
    public string? SuggestionUrn { get; set; }
}