namespace ASP.Infrastructure.Establishments.DAO
{
    public class GenderDAO
    {
        public int Code { get; }
        public string Name { get; }

        public GenderDAO(int code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
