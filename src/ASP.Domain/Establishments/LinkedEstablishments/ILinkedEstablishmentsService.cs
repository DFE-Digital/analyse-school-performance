using ASP.Core.Results;
using ASP.Domain.Establishments.UseCases.GetLinkedEstablishments;

namespace ASP.Domain.Establishments.LinkedEstablishments;

public interface ILinkedEstablishmentsService
{
    Task<Result<GetLinkedEstablishmentsResponse>> GetLinkedEstablishments(string urn);
}