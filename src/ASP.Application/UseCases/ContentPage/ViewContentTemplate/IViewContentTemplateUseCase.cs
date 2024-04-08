using ASP.Core.Results;
using ASP.Core.Templating;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.ViewContentTemplate
{
    public interface IViewContentTemplateUseCase : IUseCase<ViewContentTemplateRequest, Result<ContentTemplate>>
    {
    }
}
