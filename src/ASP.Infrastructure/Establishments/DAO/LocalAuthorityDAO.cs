namespace ASP.Infrastructure.Establishments.DAO
{
    public class LocalAuthorityDAO
    {
        public int Code { get; }
        public string Name { get; }

        public LocalAuthorityDAO(int code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
