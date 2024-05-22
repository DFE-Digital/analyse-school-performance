using ASP.Core;
using ASP.Core.Establishments;
using ASP.Core.Results;

namespace ASP.Infrastructure.Repositories
{
    public class EstablishmentRepository : IEstablishmentRepository
    {
        private const string ContainerKey = "establishments";
        private readonly IDocumentDatabase _documentDB;

        public EstablishmentRepository(IDocumentDatabase documentDB)
        {
            _documentDB = documentDB ??
                throw new ArgumentNullException(nameof(documentDB));
        }


        public Task<Result<EstablishmentDetails>> GetEstablishmentDetails(string urn)
        {
            return _documentDB.GetAsync<EstablishmentDTO>(ContainerKey, urn, urn)
                .ErrorIf(estab => estab.IsDeleted, Error.NotFound($"Establishment {urn} has been deleted."))
                .Map(dto => dto.ToEstablishment());
        }

        public Task<Result<Done>> Create(string contentId, EstablishmentDetails establishmentDetails)
        {
            return _documentDB.UpsertAsync(ContainerKey, contentId, establishmentDetails.Urn, establishmentDetails);
        }
    }
}
