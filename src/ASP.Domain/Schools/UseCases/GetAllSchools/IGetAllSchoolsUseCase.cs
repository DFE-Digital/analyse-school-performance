using ASP.Core.Pagination;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.Schools.UseCases.GetAllSchools;

public interface IGetAllSchoolsUseCase : IUseCase<GetAllSchoolsRequest, Result<ResultsPage<School>>>
{
}