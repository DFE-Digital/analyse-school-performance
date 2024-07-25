using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.ContentTemplates.UpdateContentTemplate
{
    public interface IUpdateContentTemplate : IUseCase<UpdateContentTemplateRequest, Result<Done>>
    {
    }
}
