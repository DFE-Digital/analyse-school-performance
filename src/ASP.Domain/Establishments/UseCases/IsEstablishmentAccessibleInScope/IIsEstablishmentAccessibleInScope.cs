using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.Establishments.UseCases.IsEstablishmentAccessibleInScope;

public interface IIsEstablishmentAccessibleInScope : IUseCase<IsEstablishmentAccessibleInScopeRequest, Result<IsEstablishmentAccessibleInScopeResponse>>
{
}
