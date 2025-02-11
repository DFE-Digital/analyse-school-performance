namespace ASP.Domain.Establishments
{
    public class MultiAcademyTrust
    {
        public string Id { get; }
        public string Name { get; }
        public MultiAcademyTrust(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
