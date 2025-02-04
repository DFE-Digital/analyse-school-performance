namespace ASP.Domain.Establishments.UseCases.DTO.Mapper;

public static class LinkedEstablishmentsResponseDTOMapper
{
    public static LinkedEstablishmentsResponseDTO MapToLinkedEstablishmentsResponseDTO(
        this LinkedEstablishmentsResponse linkedEstablishmentsResponse)
    {
        return new LinkedEstablishmentsResponseDTO()
        {
            Urn = linkedEstablishmentsResponse.Urn,
            Name = linkedEstablishmentsResponse.Name,
            LinkedUrns = linkedEstablishmentsResponse.LinkedUrns,
            Links = linkedEstablishmentsResponse.Links.MapToLinkResponseDTO()
        };
    }
}