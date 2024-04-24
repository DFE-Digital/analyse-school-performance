using ASP.Core.Establishments;

namespace ASP.Core.Establishments
{
    public class EstablishmentDetails
    {
        public string Urn { get; }
        public string Name { get; }

        public EstablishmentDetails(string urn, string name)
        {
            Urn = urn;
            Name = name;
        }
    }
}
