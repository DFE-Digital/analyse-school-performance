using ASP.Core.Pagination;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.Establishments.UseCases.GetAllEstablishments;

public interface IGetAllEstablishments : IUseCase<GetAllEstablishmentsRequest, Result<ResultsPage<EstablishmentListing>>>
{
}