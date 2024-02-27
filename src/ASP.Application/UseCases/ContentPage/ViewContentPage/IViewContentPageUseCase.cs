using ASP.Core.PageContent;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;
using ErrorOr;

namespace ASP.Application.UseCases.ViewContentPage
{
    public interface IViewContentPageUseCase : IUseCase<ViewContentPageRequest, ErrorOr<PageContentTemplate>>
    {
    }
}
