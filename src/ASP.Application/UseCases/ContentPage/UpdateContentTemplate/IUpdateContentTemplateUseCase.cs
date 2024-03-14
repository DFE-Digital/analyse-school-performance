using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;
using ErrorOr;

namespace ASP.Application.UseCases.UpdateContentTemplate
{
    public interface IUpdateContentTemplateUseCase : IUseCase<UpdateContentTemplateRequest, ErrorOr<UpdateContentTemplateResponse>>
    {
    }
}
