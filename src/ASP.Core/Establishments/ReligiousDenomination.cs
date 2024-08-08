namespace ASP.Core.Establishments
{
    public class ReligiousDenomination
    {
        public string Code { get; }
        public string Name { get; }

        public ReligiousDenomination(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
