using ASP.Core;
using ASP.Core.Establishments;
using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Infrastructure.DAO.Establishment;
using ASP.Infrastructure.Mapper.Establishment;

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
            return _documentDB.GetAsync<EstablishmentDetailsDAO>(ContainerKey, urn, urn)
                .ErrorIf(estab => estab.IsDeleted, Error.NotFound($"Establishment {urn} has been deleted."))
                .Map(dto => dto.MapToEstablishmentDetails());
        }
        
        public Task<Result<Done>> Create(string contentId, EstablishmentDetails establishmentDetails)
        {
            var establishmentDetailsDao = establishmentDetails.MapToEstablishmentDetailsDAO();
            
            return _documentDB.UpsertAsync(ContainerKey, contentId, establishmentDetailsDao.Urn, establishmentDetailsDao);
        }

        public async Task<Result<SearchResult<EstablishmentDetailsSearchResult>>> SearchEstablishmentNameOrLocation(
            string searchTerm, int skip, int take,
            CancellationToken cancellationToken = default)
        {
            return await _documentDB.QueryAsyncPaged<SearchResultDAO>(ContainerKey, 
                    q => q.Where(x =>
                        !x.IsDeleted &&
                        (
                            x.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                            x.Address.Street.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                            x.Address.Town.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                            x.Address.PostCode.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                        )).OrderBy(x => x.Name), skip, take,
                    cancellationToken)
                .Map(x => new SearchResult<EstablishmentDetailsSearchResult>
                {
                    Results = x.Items.MapToEstablishmentDetailsSearchResults(), Skip = skip,
                    Take = take, TotalCount = x.TotalCount, ResultCount = x.Items.Count()
                });
        }
        
        public async Task<Result<SearchResult<EstablishmentDetailsSearchResult>>> SearchLocalAuthEstablishment(string searchTerm, int skip, int take,
            CancellationToken cancellationToken = default)
        {
            return await _documentDB.QueryAsyncPaged<SearchResultDAO>(ContainerKey, 
                    q=> q.Where(x => 
                    !x.IsDeleted && x.Laestab!.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))
                        .OrderBy(x => x.Name), skip, take, cancellationToken)
                .Map(x => new SearchResult<EstablishmentDetailsSearchResult> { Results = x.Items.MapToEstablishmentDetailsSearchResults(), Skip = skip,
                    Take = take, TotalCount = x.TotalCount, ResultCount = x.Items.Count() });
        }
    }
}
