namespace ASP.Infrastructure.Establishments.DAO
{
    public class AdmissionsPolicyDAO
    {
        public int Code { get; }
        public string Name { get; }

        public AdmissionsPolicyDAO(int code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
