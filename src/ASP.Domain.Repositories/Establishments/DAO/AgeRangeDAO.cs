namespace ASP.Domain.Repositories.Establishments.DAO
{
    public class AgeRangeDAO
    {
        public string Low { get; }
        public string High { get; }

        public AgeRangeDAO(string low, string high)
        {
            Low = low;
            High = high;
        }
    }
}