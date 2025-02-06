namespace ASP.Domain.Repositories.Establishments.DAO
{
    public class ResourcedProvisionTypeDAO
    {
        public string Code { get; }
        public string Name { get; }

        public ResourcedProvisionTypeDAO(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
