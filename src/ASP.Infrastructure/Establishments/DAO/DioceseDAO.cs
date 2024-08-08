namespace ASP.Infrastructure.Establishments.DAO
{
    public class DioceseDAO
    {
        public int Code { get; }
        public string Name { get; }

        public DioceseDAO(int code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
