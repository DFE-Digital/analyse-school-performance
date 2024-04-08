using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.UpdateContentTemplate
{
    public interface IUpdateContentTemplateUseCase : IUseCase<UpdateContentTemplateRequest, Result<UpdateContentTemplateResponse>>
    {
    }
}
