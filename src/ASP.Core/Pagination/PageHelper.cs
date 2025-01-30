namespace ASP.Core.Pagination;

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

    /// <summary>
    /// Parses a string representation of a page number into an integer.
    /// </summary>
    /// <param name="page">The string representation of the page number.</param>
    /// <returns>
    /// The parsed page number as an integer if successful and greater than or equal to 1;
    /// otherwise, returns 1 as the default page number.
    /// </returns>
    /// <remarks>
    /// This method attempts to parse the input string into an integer. 
    /// If parsing is successful and the resulting value is 1 or greater, that value is returned.
    /// If parsing fails or the result is less than 1, the method returns 1 as a default value.
    /// </remarks>
    public static int ParsePageNumber(string? page)
    {
        return int.TryParse(page, out int intValue) && intValue >= 1 ? intValue : 1;
    }

}