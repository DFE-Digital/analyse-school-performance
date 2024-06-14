namespace ASP.Core.Search;

public class SearchResult<T>
{
    public IEnumerable<T> Results { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int ResultCount { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }
    
}
