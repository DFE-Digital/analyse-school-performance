namespace ASP.Domain.Repositories.Schools
{
    public class LookupValueDao
    {
        public string Code { get; }
        public string Name { get; }

        public LookupValueDao(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
