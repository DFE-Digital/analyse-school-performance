using ASP.Core.Results;

namespace ASP.Core.Establishments
{
    public interface IEstablishmentRepository
    {
        Task<Result<EstablishmentDetails>> GetEstablishmentDetails(string urn);
    }
}
