namespace ASP.Domain.Schools
{
    public class Diocese
    {
        public string Id { get; }
        public string Name { get; }
        public Diocese(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
