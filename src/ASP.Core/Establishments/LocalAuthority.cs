namespace ASP.Core.Establishments
{
    public class LocalAuthority
    {
        public string Code { get; }
        public string Name { get; }

        public LocalAuthority(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
