namespace ASP.Web.Models
{
    public class SelectSchoolViewModel
    {
        public string Search { get; set; }
        public SchoolSummary[] Establishments { get; internal set; }
        public int? Urn { get; set; }
        public bool HasSearchText => !string.IsNullOrEmpty(Search);
    }
}

