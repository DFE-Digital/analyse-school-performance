namespace ASP.Core.Search;

public class SearchResult<T>
{
    public IEnumerable<T> Results { get; set; } = new List<T>();
    public int TotalResults { get; set; }
    public int ResultsPerPage { get; set; }
    public int Page { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
}
