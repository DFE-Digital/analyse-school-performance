using ASP.Core.Results;

namespace ASP.Domain.Establishments.UseCases.GetEstablishmentDetails
{
    public class GetEstablishmentDetails : IGetEstablishmentDetails
    {
        private readonly IEstablishmentRepository _repository;

        public GetEstablishmentDetails(IEstablishmentRepository pageContentRepository)
        {
            _repository = pageContentRepository ??
                          throw new ArgumentNullException(nameof(pageContentRepository));
        }

        public Task<Result<EstablishmentDetails>> HandleRequest(GetEstablishmentDetailsRequest request)
        {
            return
                from establishment in _repository.GetEstablishmentDetails(request.Urn)
                select establishment;
        }
    }
}