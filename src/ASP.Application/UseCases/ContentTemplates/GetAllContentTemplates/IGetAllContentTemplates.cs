using ASP.Core.Results;
using ASP.Core.Templating;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.ContentPage.GetAllContentTemplates
{
    public interface IGetAllContentTemplates : IUseCase<Result<List<ContentTemplate>>>
    {
    }
}
