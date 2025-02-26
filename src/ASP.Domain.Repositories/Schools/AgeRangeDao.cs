namespace ASP.Domain.Repositories.Schools
{
    public class AgeRangeDao
    {
        public string Low { get; }
        public string High { get; }

        public AgeRangeDao(string low, string high)
        {
            Low = low;
            High = high;
        }
    }
}