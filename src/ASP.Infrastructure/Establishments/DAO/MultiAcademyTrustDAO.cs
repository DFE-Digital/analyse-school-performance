namespace ASP.Infrastructure.Establishments.DAO
{
    public class MultiAcademyTrustDAO
    {
        public int Uid { get; }
        public string Name { get; }

        public MultiAcademyTrustDAO(int uid, string name)
        {
            Uid = uid;
            Name = name;
        }
    }
}
