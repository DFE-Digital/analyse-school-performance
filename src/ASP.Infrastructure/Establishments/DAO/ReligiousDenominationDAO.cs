namespace ASP.Infrastructure.Establishments.DAO
{
    public class ReligiousDenominationDAO
    {
        public string Code { get; }
        public string Name { get; }

        public ReligiousDenominationDAO(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
