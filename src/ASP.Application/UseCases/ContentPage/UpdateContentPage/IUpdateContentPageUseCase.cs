using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;
using ErrorOr;

namespace ASP.Application.UseCases.UpdateContentPage
{
    public interface IUpdateContentPageUseCase : IUseCase<UpdateContentPageRequest, ErrorOr<UpdateContentPageResponse>>
    {
    }
}
