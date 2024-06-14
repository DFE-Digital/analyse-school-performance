namespace ASP.Infrastructure.DAO.Establishment
{
    public class Gender
    {
        public int Code { get; }
        public string Name { get; }

        public Gender(int code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
