namespace ASP.Web.Core.ErrorHandling
{
    public class ErrorHandlingOptions
    {
        public const string SectionName = "ErrorHandling";

        public bool ShowStackTrace { get; set; }
        public bool ForceProductionErrorPage { get; set; }
    }
}
