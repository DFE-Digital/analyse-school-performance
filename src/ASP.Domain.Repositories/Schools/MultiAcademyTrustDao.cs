namespace ASP.Domain.Repositories.Schools
{
    public class MultiAcademyTrustDao
    {
        public string Uid { get; }
        public string Name { get; }

        public MultiAcademyTrustDao(string uid, string name)
        {
            Uid = uid;
            Name = name;
        }
    }
}
