namespace ASP.Infrastructure.Repositories.Establishments.DAO
{
    public class AdmissionsPolicyDAO
    {
        public string Code { get; }
        public string Name { get; }

        public AdmissionsPolicyDAO(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
