using ASP.Core.Results;
using ASP.Core.Templating;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.ContentTemplates.ViewContentTemplate
{
    public interface IViewContentTemplate : IUseCase<ViewContentTemplateRequest, Result<ContentTemplate>>
    {
    }
}
