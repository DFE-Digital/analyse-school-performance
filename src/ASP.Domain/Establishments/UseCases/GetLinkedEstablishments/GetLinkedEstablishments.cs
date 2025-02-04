using ASP.Core.Results;
using ASP.Domain.Establishments.GetLinkedEstablishments;
using ASP.Domain.Establishments.UseCases.DTO;
using ASP.Domain.Establishments.UseCases.DTO.Mapper;

namespace ASP.Domain.Establishments.UseCases.GetLinkedEstablishments;

public class GetLinkedEstablishments : IGetLinkedEstablishments
{
    private readonly ILinkedEstablishmentsService _linkedEstablishmentsService;

    public GetLinkedEstablishments(ILinkedEstablishmentsService linkedEstablishmentsService)
    {
        _linkedEstablishmentsService = linkedEstablishmentsService ??
                                       throw new ArgumentNullException(nameof(linkedEstablishmentsService));
    }

    public Task<Result<LinkedEstablishmentsResponseDTO>> HandleRequest(GetLinkedEstablishmentsRequest request)
    {
        return from response in _linkedEstablishmentsService.GetLinkedEstablishments(request.Urn)
            select response.MapToLinkedEstablishmentsResponseDTO();
    }
}