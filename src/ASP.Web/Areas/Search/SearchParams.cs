using System.ComponentModel.DataAnnotations;
using ASP.Core;

namespace ASP.Web.Areas.Search;

public class SearchParams
{ 
    [Required(ErrorMessage = Constants.SchoolSearchTermInputValidationMessage)]
    public string SearchTerm { get; set; }
    public int Page { get; set; }
}