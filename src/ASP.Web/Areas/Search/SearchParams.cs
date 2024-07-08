using System.ComponentModel.DataAnnotations;
using ASP.Core;

namespace ASP.Web.Areas.Search;

public class SearchParams
{ 
    [Required(ErrorMessage = Constants.SchoolSearchTermShortValidationMessage)]
    public string? SearchTerm { get; set; }
    public int Page { get; set; }
}