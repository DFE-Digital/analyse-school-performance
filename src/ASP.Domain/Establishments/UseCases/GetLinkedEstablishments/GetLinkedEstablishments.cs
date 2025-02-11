using ASP.Core.Results;
using ASP.Domain.Establishments.LinkedEstablishments;

namespace ASP.Domain.Establishments.UseCases.GetLinkedEstablishments;

public class GetLinkedEstablishments : IGetLinkedEstablishments
{
    private readonly ILinkedEstablishmentsService _linkedEstablishmentsService;

    public GetLinkedEstablishments(ILinkedEstablishmentsService linkedEstablishmentsService)
    {
        _linkedEstablishmentsService = linkedEstablishmentsService ??
                                       throw new ArgumentNullException(nameof(linkedEstablishmentsService));
    }

    public Task<Result<GetLinkedEstablishmentsResponse>> HandleRequest(GetLinkedEstablishmentsRequest request)
    {
        return from response in _linkedEstablishmentsService.GetLinkedEstablishments(request.Urn)
            select response;
    }
}