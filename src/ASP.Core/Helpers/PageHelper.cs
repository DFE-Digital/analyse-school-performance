namespace ASP.Core.Helpers;

public static class PageHelper
{
    public static (int Skip, int Take, int ValidPage) ConstructPagingRequest(int totalCount, int page, 
        int itemsPerSearchPage = Constants.SearchResultPageSize)
    {
        // Calculate the total number of pages
        var totalPages = (int)Math.Ceiling((double)totalCount / itemsPerSearchPage);
        // Adjust the requested page to be within the valid range
        var validPage = Math.Max(1, Math.Min(totalPages, page));
        var skip = Math.Max(0, validPage - 1) * itemsPerSearchPage;
        var take = itemsPerSearchPage;

        return (skip, take, validPage);
    }

}