namespace ASP.Domain
{
    public class LookupValue
    {
        public string Code { get; }
        public string Name { get; }

        public LookupValue(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
