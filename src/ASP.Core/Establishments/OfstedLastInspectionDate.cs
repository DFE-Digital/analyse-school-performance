namespace ASP.Core.Establishments
{
    public class OfstedLastInspectionDate
    {
        public DateOnly LastInspectionDate { get; }

        public OfstedLastInspectionDate(DateOnly lastInspectionDate)
        {
            LastInspectionDate = lastInspectionDate;
        }
    }
}
