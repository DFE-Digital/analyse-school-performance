using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.Schools.UseCases.IsSchoolAccessibleInScope;

public interface IIsSchoolAccessibleInScopeUseCase : IUseCase<IsSchoolAccessibleInScopeRequest, Result<IsSchoolAccessibleInScopeResponse>>
{
}
