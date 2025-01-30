namespace ASP.Domain.Establishments
{
    public class ResourcedProvisionType
    {
        public string Code { get; }
        public string Name { get; }

        public ResourcedProvisionType(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
