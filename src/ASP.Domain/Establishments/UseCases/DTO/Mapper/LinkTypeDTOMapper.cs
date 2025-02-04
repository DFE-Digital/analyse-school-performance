namespace ASP.Domain.Establishments.UseCases.DTO.Mapper;

public static class LinkTypeDTOMapper
{
    public static LinkTypeDTO MapToLinkTypeDTO(
        this LinkType? linkType)
    {
        if (linkType == null) return new LinkTypeDTO();
        return new LinkTypeDTO()
        {
            Name = linkType.Name,
            Code = linkType.Code
        };
    }
}