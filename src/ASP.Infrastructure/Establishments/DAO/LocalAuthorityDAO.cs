namespace ASP.Infrastructure.Establishments.DAO
{
    public class LocalAuthorityDAO
    {
        public string Code { get; }
        public string Name { get; }

        public LocalAuthorityDAO(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
