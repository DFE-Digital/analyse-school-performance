using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;
using ASP.Core.Pagination;

namespace ASP.Domain.Schools.UseCases.SchoolSearch
{
    public interface ISchoolSearchUseCase : IUseCase<SchoolSearchRequest, Result<ResultsPage<School>>>
    {
    }
}
