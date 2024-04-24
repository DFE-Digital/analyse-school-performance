using ASP.Core.Establishments;
using ASP.Core.Templating;
using MR;

namespace ASP.Infrastructure.Repositories
{
    public class EstablishmentDTO
    {
        public string Urn { get; set; } = null!;
        public string Name { get; set; } = null!;

        internal EstablishmentDetails ToEstablishment()
        {
            return new EstablishmentDetails(
                Urn,
                Name
            );
        }

    }
}
