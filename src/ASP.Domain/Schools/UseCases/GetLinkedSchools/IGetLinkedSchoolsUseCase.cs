using ASP.Core.Results;
using ASP.Domain.Schools.LinkedSchools;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.Schools.UseCases.GetLinkedSchools;

public interface IGetLinkedSchoolsUseCase : IUseCase<GetLinkedSchoolsRequest, Result<List<LinkedSchoolsLink>>>
{
}