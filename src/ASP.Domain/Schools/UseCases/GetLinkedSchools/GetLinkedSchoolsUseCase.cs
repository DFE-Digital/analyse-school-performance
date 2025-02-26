using ASP.Core.Results;

namespace ASP.Domain.Schools.UseCases.GetLinkedSchools;

public class GetLinkedSchoolsUseCase : IGetLinkedSchoolsUseCase
{
    private readonly ISchoolRepository _repository;

    public GetLinkedSchoolsUseCase(ISchoolRepository schoolRepository)
    {
        _repository = schoolRepository;
    }

    public Task<Result<GetLinkedSchoolsResponse>> HandleRequest(GetLinkedSchoolsRequest request)
    {
        return
            from urn in SchoolUrn.Parse(request.Urn)
            from school in _repository.GetWithLinkedSchools(urn)
            select new GetLinkedSchoolsResponse(
                school.LinkedUrns,
                school.Links);
    }
}