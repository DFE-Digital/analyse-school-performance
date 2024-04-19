using ASP.Core.Results;

namespace ASP.Core.Establishments.Repository
{
    public interface IEstablishmentRepository
    {
        Task<Result<EstablishmentDetails>> GetEstablishmentDetails(string urn);

    }
}
