using ASP.Core.Establishments;
using ASP.Core.Results;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.DTO.Mapper;

namespace ASP.Application.UseCases.Establishments.GetEstablishmentDetails
{
    public class GetEstablishmentDetails : IGetEstablishmentDetails
    {
        private readonly IEstablishmentRepository _repository;

        public GetEstablishmentDetails(IEstablishmentRepository pageContentRepository)
        {
            _repository = pageContentRepository ??
                          throw new ArgumentNullException(nameof(pageContentRepository));
        }

        public async Task<Result<EstablishmentDetailsDTO>> HandleRequest(GetEstablishmentDetailsRequest request)
        {
            return await _repository.GetEstablishmentDetails(request.Urn).Map(x =>
                x.MapToEstablishmentDetailsDTO());
        }
    }
}