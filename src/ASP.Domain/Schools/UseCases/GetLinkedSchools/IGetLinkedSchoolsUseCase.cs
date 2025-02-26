using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.Schools.UseCases.GetLinkedSchools;

public interface IGetLinkedSchoolsUseCase : IUseCase<GetLinkedSchoolsRequest, Result<GetLinkedSchoolsResponse>>
{
}