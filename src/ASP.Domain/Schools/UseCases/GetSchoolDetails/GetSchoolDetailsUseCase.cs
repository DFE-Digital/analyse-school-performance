using ASP.Core.Results;
using ASP.Domain.Schools.Details;

namespace ASP.Domain.Schools.UseCases.GetSchoolDetails
{
    public class GetSchoolDetailsUseCase : IGetSchoolDetailsUseCase
    {
        private readonly ISchoolRepository _repository;

        public GetSchoolDetailsUseCase(ISchoolRepository pageContentRepository)
        {
            _repository = pageContentRepository ??
                          throw new ArgumentNullException(nameof(pageContentRepository));
        }

        public Task<Result<SchoolWithEstablishmentDetails>> HandleRequest(GetSchoolDetailsRequest request)
        {
            return
                from urn in SchoolUrn.Parse(request.Urn)
                from school in _repository.GetWithEstablishmentDetails(urn)
                select school;
        }
    }
}