namespace ASP.Web.Areas.Shared.Pagination;

public class PaginationModel
{
    public string PageLinkBaseUrl { get; }
    public int CurrentPage { get; }
    public int TotalResults { get; }
    public int ResultsPerPage { get; }
    public string ResultNameSingular { get; }
    public string ResultNamePlural { get; }

    public PaginationModel(string pageLinkBaseUrl, int currentPage, int totalResults, int resultsPerPage, string resultNameSingular, string resultNamePlural)
    {
        PageLinkBaseUrl = string.Format("{0}{1}page=", pageLinkBaseUrl, pageLinkBaseUrl.Contains("?") ? "&" : "?");
        CurrentPage = currentPage;
        TotalResults = totalResults;
        ResultsPerPage = resultsPerPage;
        ResultNameSingular = resultNameSingular;
        ResultNamePlural = resultNamePlural;
    }

    public int TotalPages => (int)Math.Ceiling((double)TotalResults / ResultsPerPage);
    public bool ShowPagination => TotalPages > 1;
    public bool ShowPreviousLink => CurrentPage > 1;
    public string PageLink(int page) => $"{PageLinkBaseUrl}{page}";
    public string PreviousLink => PageLink(CurrentPage - 1);
    public bool ShowNextLink => CurrentPage < TotalPages;
    public string NextLink => PageLink(CurrentPage + 1);
    public int Skip => Math.Max(0, CurrentPage - 1) * ResultsPerPage;
    public int ResultsStartOffset => Skip + 1;
    public int ResultsEndOffset => Skip + ResultsPerPage > TotalResults ? TotalResults : Skip + ResultsPerPage;

    public IEnumerable<(int, string)> PageLinks
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

            return Enumerable.Range(startPage, Math.Min(TotalPages, endPage) - startPage + 1).Select(p => (p, PageLink(p)));
        }
    }
}