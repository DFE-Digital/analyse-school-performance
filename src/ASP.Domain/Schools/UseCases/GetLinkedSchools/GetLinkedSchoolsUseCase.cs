using ASP.Core.Results;
using ASP.Domain.Schools.LinkedSchools;

namespace ASP.Domain.Schools.UseCases.GetLinkedSchools;

public class GetLinkedSchoolsUseCase : IGetLinkedSchoolsUseCase
{
    private readonly ISchoolRepository _repository;

    public GetLinkedSchoolsUseCase(ISchoolRepository schoolRepository)
    {
        _repository = schoolRepository;
    }

    public Task<Result<List<LinkedSchoolsLink>>> HandleRequest(GetLinkedSchoolsRequest request)
    {
        return
            from urn in SchoolUrn.Parse(request.Urn)
            from school in _repository.GetWithLinkedSchools(urn)
            select school.Links.ToList();
    }
}