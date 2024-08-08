namespace ASP.Infrastructure.Establishments.DAO
{
    public class DioceseDAO
    {
        public string Code { get; }
        public string Name { get; }

        public DioceseDAO(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
