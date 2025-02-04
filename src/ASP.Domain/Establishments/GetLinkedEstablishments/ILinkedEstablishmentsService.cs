using ASP.Core.Results;

namespace ASP.Domain.Establishments.GetLinkedEstablishments;

public interface ILinkedEstablishmentsService
{
    Task<Result<LinkedEstablishmentsResponse>> GetLinkedEstablishments(string urn);
}