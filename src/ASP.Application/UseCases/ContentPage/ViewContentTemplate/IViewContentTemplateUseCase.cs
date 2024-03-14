using ASP.Core.Templating;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;
using ErrorOr;

namespace ASP.Application.UseCases.ViewContentTemplate
{
    public interface IViewContentTemplateUseCase : IUseCase<ViewContentTemplateRequest, ErrorOr<ContentTemplate>>
    {
    }
}
