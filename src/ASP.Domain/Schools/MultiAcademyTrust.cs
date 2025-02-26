namespace ASP.Domain.Schools
{
    public class MultiAcademyTrust
    {
        public string Uid { get; }
        public string Name { get; }
        public MultiAcademyTrust(string uid, string name)
        {
            Uid = uid;
            Name = name;
        }
    }
}
