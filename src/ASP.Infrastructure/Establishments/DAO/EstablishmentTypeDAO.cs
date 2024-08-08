namespace ASP.Infrastructure.Establishments.DAO
{
    public class EstablishmentTypeDAO
    {
        public string Code { get; }
        public string Name { get; }

        public EstablishmentTypeDAO(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
