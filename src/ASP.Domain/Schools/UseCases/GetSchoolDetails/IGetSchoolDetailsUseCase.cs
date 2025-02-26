using ASP.Core.Results;
using ASP.Domain.Schools.Details;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.Schools.UseCases.GetSchoolDetails
{
    public interface IGetSchoolDetailsUseCase : IUseCase<GetSchoolDetailsRequest, Result<SchoolWithEstablishmentDetails>>
    {
    }
}
