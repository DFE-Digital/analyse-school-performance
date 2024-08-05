namespace ASP.Infrastructure.Establishments.DAO
{
    public class ReligiousDenominationDAO
    {
        public int Code { get; }
        public string Name { get; }

        public ReligiousDenominationDAO(int code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
