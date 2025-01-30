using ASP.Core.Results;
using ASP.Domain.Establishments.UseCases.DTO;
using ASP.Domain.Establishments.UseCases.DTO.Mapper;

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

        public Task<Result<EstablishmentDetailsDTO>> HandleRequest(GetEstablishmentDetailsRequest request)
        {
            return
                from establishment in _repository.GetEstablishmentDetails(request.Urn)
                select establishment.MapToEstablishmentDetailsDTO();
        }
    }
}