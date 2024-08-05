using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads
{
    public interface IGetAvailableSchoolDownloads : IUseCase<GetAvailableSchoolDownloadsRequest, Result<GetAvailableSchoolDownloadsResponse>>
    {
    }
}
