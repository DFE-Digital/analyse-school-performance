namespace ASP.Infrastructure.DAO.Establishment
{
    public class EstablishmentType
    {
        public int Code { get; }
        public string Name { get; }

        public EstablishmentType(int code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
