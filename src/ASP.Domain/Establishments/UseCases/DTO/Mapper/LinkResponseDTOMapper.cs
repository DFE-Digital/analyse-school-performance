namespace ASP.Domain.Establishments.UseCases.DTO.Mapper;

public static class LinkResponseDTOMapper
{
    public static List<LinkResponseDTO> MapToLinkResponseDTO(
        this List<LinkResponse> linkResponse)
    {
        return linkResponse.Select(x => new LinkResponseDTO()
        {
            Date = x.Date?.ToString("yyyy-MM-dd"),
            LinkType = x.LinkType.MapToLinkTypeDTO(),
            Establishments = x.Establishments.MapToLinkedEstablishmentDTO(),
            Description = x.Description
        }).ToList();
    }
}