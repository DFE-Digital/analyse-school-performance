namespace ASP.Domain.Establishments.UseCases.DTO.Mapper;

public static class LinkedEstablishmentDTOMapper
{
    public static List<LinkedEstablishmentDTO> MapToLinkedEstablishmentDTO(
        this List<LinkedEstablishment>? linkedEstablishment)
    {
        if (linkedEstablishment == null) return new List<LinkedEstablishmentDTO>();
        
        return linkedEstablishment.Select(x => new LinkedEstablishmentDTO()
        {
            Name = x.Name,
            Urn = x.Urn
        }).ToList();
    }
}