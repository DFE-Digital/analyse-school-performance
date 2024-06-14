namespace ASP.Core.Helpers;

public static class PageHelper
{
    public static (int Skip, int Take) ConstructPagingRequest(int page, 
        int itemsPerSearchPage = Constants.SearchResultPageSize)
    {
        int skip = Math.Max(0, page - 1) * itemsPerSearchPage;
        int take = itemsPerSearchPage;

        return (skip, take);
    }

}