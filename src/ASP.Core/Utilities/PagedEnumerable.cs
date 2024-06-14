namespace ASP.Core.Utilities;

public class PagedEnumerable<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }

    public PagedEnumerable(IEnumerable<T> items, int totalCount)
    {
        Items = items;
        TotalCount = totalCount;
    }

}