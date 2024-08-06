namespace ASP.Core.Establishments
{
    public class AdmissionsPolicy
    {
        public int Code { get; set;}
        public string Name { get; set;}

        public AdmissionsPolicy(int code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
