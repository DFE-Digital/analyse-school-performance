using ASP.Core.Results;
using ASP.Core.Templating;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.ContentPage.GetAllAllContentTemplates
{
    public interface IGetAllContentTemplatesUseCase : IUseCase<Result<List<ContentTemplate>>>
    {
    }
}
