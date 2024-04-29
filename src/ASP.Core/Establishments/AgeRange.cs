namespace ASP.Core.Establishments
{
    public class AgeRange
    {
        public string Low { get; }
        public string High { get; }

        public AgeRange(string low, string high)
        {
            Low = low;
            High = high;
        }
    }
}