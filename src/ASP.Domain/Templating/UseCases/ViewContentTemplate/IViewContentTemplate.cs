using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.Templating.UseCases.ViewContentTemplate
{
    public interface IViewContentTemplate : IUseCase<ViewContentTemplateRequest, Result<ContentTemplate>>
    {
    }
}
