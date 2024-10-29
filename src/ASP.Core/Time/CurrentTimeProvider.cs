namespace ASP.Core.Time
{
    public class CurrentTimeProvider
    {
        public DateTime? Override { get; set; }
        public DateTime CurrentTime => Override ?? DateTime.UtcNow;
    }
}