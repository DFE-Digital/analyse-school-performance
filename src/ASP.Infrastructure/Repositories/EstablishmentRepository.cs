using ASP.Core;
using ASP.Core.Results;
using ASP.Core.Establishments;
using ASP.Core.Establishments.Repository;

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
               .Map(dto => dto.ToEstablishment());
        }
    }
}
