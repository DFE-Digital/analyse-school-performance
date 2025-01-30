namespace ASP.Infrastructure.Repositories.Establishments.DAO
{
    public class GenderDAO
    {
        public string Code { get; }
        public string Name { get; }

        public GenderDAO(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
