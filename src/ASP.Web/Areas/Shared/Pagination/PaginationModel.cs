using ASP.Core;

namespace ASP.Web.Areas.Shared.Pagination;

public class PaginationModel
{
    public string SearchTerm { get; set; } = string.Empty;
    public int TotalCount { get; set; }
    public int ResultCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; } = Constants.SearchResultPageSize; // Default page size
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool ShowPagination => TotalPages > 1;
    public bool ShowPreviousLink => CurrentPage > 1;
    public bool ShowNextLink => CurrentPage < TotalPages;

    public IEnumerable<int> PageLinks
    {
        get
        {
            var totalDisplayPages = 5; // Total pages to display in the pagination
            var halfPageShow = totalDisplayPages / 2;

            var startPage = CurrentPage - halfPageShow;
            var endPage = CurrentPage + halfPageShow;

            // Adjust the start and end pages if they are out of bounds
            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1); // Adjust end page when the start page is less than 1
                startPage = 1;
            }

            if (endPage > TotalPages)
            {
                endPage = TotalPages;
                if (endPage > totalDisplayPages)
                {
                    startPage = endPage - totalDisplayPages + 1;
                }
            }

            return Enumerable.Range(startPage, Math.Min(TotalPages, endPage) - startPage + 1);
        }
    }


    public int Skip { get; set; }
    public int ResultsStartOffset => Skip + 1;

    public int ResultsEndOffset => Skip + ResultCount;
}