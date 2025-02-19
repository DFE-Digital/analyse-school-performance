using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.Templating.UseCases.GetContentTemplate
{
    public interface IGetContentTemplate : IUseCase<GetContentTemplateRequest, Result<ContentTemplate>>
    {
    }
}
