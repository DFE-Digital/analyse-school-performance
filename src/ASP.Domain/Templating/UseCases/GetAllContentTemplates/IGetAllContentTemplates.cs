using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.Templating.UseCases.GetAllContentTemplates
{
    public interface IGetAllContentTemplates : IUseCase<Result<List<ContentTemplate>>>
    {
    }
}
