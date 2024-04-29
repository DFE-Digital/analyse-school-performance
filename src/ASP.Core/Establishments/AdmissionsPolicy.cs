namespace ASP.Core.Establishments
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
