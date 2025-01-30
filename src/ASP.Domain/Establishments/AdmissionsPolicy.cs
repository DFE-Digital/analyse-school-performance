namespace ASP.Domain.Establishments
{
    public class AdmissionsPolicy
    {
        public string Code { get; set;}
        public string Name { get; set;}

        public AdmissionsPolicy(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
