namespace ASP.Web.Models
{
    public class SchoolSummary
    {
        public int Urn { get; set; }
        public string Name { get; set; }
        public string LAESTAB { get; set; }

        public SchoolSummary(int urn, string name, string laestab)
        {
            Urn = urn;
            Name = name;
            LAESTAB = laestab;
        }
    }
}
