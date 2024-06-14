namespace ASP.Infrastructure.DAO.Establishment
{
    public class LocalAuthority
    {
        public int Code { get; }
        public string Name { get; }

        public LocalAuthority(int code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
