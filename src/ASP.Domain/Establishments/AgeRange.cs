namespace ASP.Domain.Establishments
{
    public class AgeRange
    {
        public string Low { get; set; }
        public string High { get; set; }
        
        public AgeRange(string low, string high)
        {
            Low = low;
            High = high;
        }
    }
}