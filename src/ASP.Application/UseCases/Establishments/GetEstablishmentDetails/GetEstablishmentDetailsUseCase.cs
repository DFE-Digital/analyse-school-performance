using ASP.Core.Establishments;
using ASP.Core.Results;

namespace ASP.Application.UseCases.GetEstablishmentDetails
{
    public class GetEstablishmentDetailsUseCase : IGetEstablishmentDetailsUseCase
    {
        private readonly IEstablishmentRepository _repository;

        public GetEstablishmentDetailsUseCase(IEstablishmentRepository pageContentRepository)
        {
            _repository = pageContentRepository ??
                throw new ArgumentNullException(nameof(pageContentRepository));
        }

        public async Task<Result<EstablishmentDetails>> HandleRequest(GetEstablishmentDetailsUseCaseRequest request)
        {
            return await _repository.GetEstablishmentDetails(request.ContentTemplateId);
        }
    }
}
