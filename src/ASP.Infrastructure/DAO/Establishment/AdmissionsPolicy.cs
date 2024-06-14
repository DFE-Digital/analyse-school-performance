namespace ASP.Infrastructure.DAO.Establishment
{
    public class AdmissionsPolicy
    {
        public int Code { get; }
        public string Name { get; }

        public AdmissionsPolicy(int code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
