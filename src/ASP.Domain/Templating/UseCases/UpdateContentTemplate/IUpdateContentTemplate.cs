using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.Templating.UseCases.UpdateContentTemplate
{
    public interface IUpdateContentTemplate : IUseCase<UpdateContentTemplateRequest, Result<Done>>
    {
    }
}
