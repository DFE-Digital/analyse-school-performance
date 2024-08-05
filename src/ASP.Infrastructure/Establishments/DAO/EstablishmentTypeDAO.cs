namespace ASP.Infrastructure.Establishments.DAO
{
    public class EstablishmentTypeDAO
    {
        public int Code { get; }
        public string Name { get; }

        public EstablishmentTypeDAO(int code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
