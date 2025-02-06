namespace ASP.Domain.Repositories.Establishments.DAO
{
    public class MultiAcademyTrustDAO
    {
        public string Uid { get; }
        public string Name { get; }

        public MultiAcademyTrustDAO(string uid, string name)
        {
            Uid = uid;
            Name = name;
        }
    }
}
