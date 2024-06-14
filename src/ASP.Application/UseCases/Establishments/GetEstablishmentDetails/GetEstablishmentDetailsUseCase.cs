using ASP.Core.DTO.Establishment;
using ASP.Core.Establishments;
using ASP.Core.Mapper.Establishment;
using ASP.Core.Results;

namespace ASP.Application.UseCases.Establishments.GetEstablishmentDetails
{
    public class GetEstablishmentDetailsUseCase : IGetEstablishmentDetailsUseCase
    {
        private readonly IEstablishmentRepository _repository;

        public GetEstablishmentDetailsUseCase(IEstablishmentRepository pageContentRepository)
        {
            _repository = pageContentRepository ??
                throw new ArgumentNullException(nameof(pageContentRepository));
        }

        public async Task<Result<EstablishmentDetailsDTO>> HandleRequest(GetEstablishmentDetailsUseCaseRequest request)
        {
            return await _repository.GetEstablishmentDetails(request.ContentTemplateId).Map(x => 
                x.MapToEstablishmentDetailsDTO() );
        }
    }
}
