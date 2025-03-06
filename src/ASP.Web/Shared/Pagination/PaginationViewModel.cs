namespace ASP.Web.Shared.Pagination;

/// <summary>
/// Represents a model for handling pagination in web applications.
/// </summary>
public class PaginationViewModel
{
    private const string QueryStringPageParameter = "page=";

    // Constants for pagination display configuration
    private const int DISTANCE_FROM_CURRENT = 2; // Distance threshold from current page

    #region Properties

    public string PageLinkBaseUrl { get; }
    public int CurrentPage { get; }
    public int TotalResults { get; }
    public int ResultsPerPage { get; }
    public string ResultNameSingular { get; }
    public string ResultNamePlural { get; }

    public int TotalPages { get; }
    public bool ShowPagination { get; }
    public bool ShowPreviousLink { get; }
    public bool ShowNextLink { get; }
    public string PreviousLink { get; }
    public string NextLink { get; }

    public int ResultsStartOffset { get; }
    public int ResultsEndOffset { get; }

    #endregion

    #region Constructor

    public PaginationViewModel(
        string pageLinkBaseUrl,
        int currentPage,
        int totalResults,
        int resultsPerPage,
        string resultNameSingular,
        string resultNamePlural)
    {
        PageLinkBaseUrl = BuildBaseUrl(pageLinkBaseUrl);
        TotalResults = totalResults;
        ResultsPerPage = resultsPerPage;
        ResultNameSingular = resultNameSingular;
        ResultNamePlural = resultNamePlural;

        TotalPages = (int)Math.Ceiling((double)TotalResults / ResultsPerPage);
        CurrentPage = Math.Min(currentPage, TotalPages);

        ShowPagination = TotalPages > 1;
        ShowPreviousLink = CurrentPage > 1;
        ShowNextLink = CurrentPage < TotalPages;
        PreviousLink = PageLink(CurrentPage - 1);
        NextLink = PageLink(CurrentPage + 1);

        var skip = Math.Max(0, CurrentPage - 1) * ResultsPerPage;
        ResultsStartOffset = skip + 1;
        ResultsEndOffset = Math.Min(skip + ResultsPerPage, TotalResults);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Generates a URL for a specific page number.
    /// </summary>
    public string PageLink(int page) => $"{PageLinkBaseUrl}{page}";

    /// <summary>
    /// Gets the collection of pagination items for display.
    /// </summary>
    public IEnumerable<PaginationItem> GetPageLinks() =>
        TotalPages <= 1 ? Enumerable.Empty<PaginationItem>() : GeneratePageLinks();

    #endregion

    #region Private Methods

    private static string BuildBaseUrl(string pageLinkBaseUrl)
    {
        var separator = pageLinkBaseUrl.Contains("?") ? "&" : "?";
        return $"{pageLinkBaseUrl}{separator}{QueryStringPageParameter}";
    }

    /// <summary>
    /// Gets the collection of pagination items for display.
    /// Implements the following pattern:
    /// - Page 1: [1] 2 … 100
    /// - Page 2: 1 [2] 3 … 100
    /// - Page 3: 1 2 [3] 4 … 100
    /// - Page 4: 1 2 3 [4] 5 … 100
    /// - Page 5: 1 … 4 [5] 6 … 100
    /// - Page 98: 1 … 97 [98] 99 100
    /// - Page 99: 1 … 98 [99] 100
    /// - Page 100: 1 … 99 [100]
    /// </summary>
    private IEnumerable<PaginationItem> GeneratePageLinks()
    {
        var links = new List<PaginationItem>
        {
            // Add first page
            CreatePageItem(1)
        };

        // Add middle pages
        for (var page = 2; page < TotalPages; page++)
        {
            if (ShouldSkipPage(page))
                continue;

            if (ShouldAddEllipsis(page))
                links.Add(CreateEllipsisItem());

            if (IsWithinCurrentPageRange(page))
                links.Add(CreatePageItem(page));
        }

        // Add last page if not already added
        if (TotalPages > 1)
            links.Add(CreatePageItem(TotalPages));

        return links;
    }

    /// <summary>
    /// Determines if a page number should be skipped in the pagination display.
    /// Pages that are too far from the current page (beyond DISTANCE_FROM_CURRENT) are skipped.
    /// </summary>
    /// <param name="pageToCheck">The page number being evaluated</param>
    /// <returns>True if the page should be skipped, false otherwise</returns>
    private bool ShouldSkipPage(int pageToCheck)
    {
        var minimumPageToShow = CurrentPage - DISTANCE_FROM_CURRENT;
        var maximumPageToShow = CurrentPage + DISTANCE_FROM_CURRENT;

        return pageToCheck < minimumPageToShow || pageToCheck > maximumPageToShow;
    }

    /// <summary>
    /// Determines if an ellipsis (...) should be added at this position in the pagination.
    /// Ellipsis are added at the boundaries of the visible page range, except when:
    /// - The page is 2 (no need for ellipsis right after page 1)
    /// - The page is second-to-last (no need for ellipsis right before the last page)
    /// </summary>
    /// <param name="pageToCheck">The page number being evaluated</param>
    /// <returns>True if an ellipsis should be added, false otherwise</returns>
    private bool ShouldAddEllipsis(int pageToCheck)
    {
        var lowerEllipsisPoint = CurrentPage - DISTANCE_FROM_CURRENT;
        var upperEllipsisPoint = CurrentPage + DISTANCE_FROM_CURRENT;
        var isSecondToLastPage = pageToCheck == TotalPages - 1;

        // Don't show ellipsis for page 2 or second-to-last page
        if (pageToCheck == 2 || isSecondToLastPage)
        {
            return false;
        }

        return pageToCheck == lowerEllipsisPoint || pageToCheck == upperEllipsisPoint;
    }

    /// <summary>
    /// Determines if a page number is within the visible range around the current page.
    /// Example: for current page 5, with DISTANCE_FROM_CURRENT = 2, 
    /// the visible range would be pages 3,4,5,6,7
    /// </summary>
    /// <param name="pageToCheck">The page number being evaluated</param>
    /// <returns>True if the page is within the visible range, false otherwise</returns>
    private bool IsWithinCurrentPageRange(int pageToCheck)
    {
        var minimumVisiblePage = CurrentPage - DISTANCE_FROM_CURRENT;
        var maximumVisiblePage = CurrentPage + DISTANCE_FROM_CURRENT;

        return pageToCheck > minimumVisiblePage && pageToCheck < maximumVisiblePage;
    }

    private PaginationItem CreatePageItem(int pageNumber) => new() {
        Number = pageNumber.ToString(),
        Current = CurrentPage == pageNumber,
        Href = PageLink(pageNumber),
        Ellipsis = false
    };

    private PaginationItem CreateEllipsisItem() => new() {
        Ellipsis = true
    };

    #endregion

    #region Nested Types

    public class PaginationItem
    {
        public string? Number { get; set; }
        public bool Current { get; set; }
        public string? Href { get; set; }
        public bool Ellipsis { get; set; }
    }

    #endregion
}