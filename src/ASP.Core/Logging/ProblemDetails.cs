namespace ASP.Core.Logging
{
    public class ProblemDetails
    {
        public int? StatusCode { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }
        public string? Detail { get; set; }
    }
}