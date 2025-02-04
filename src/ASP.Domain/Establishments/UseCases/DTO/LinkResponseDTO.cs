namespace ASP.Domain.Establishments.UseCases.DTO;

public class LinkResponseDTO
{
    public string? Date { get; set; }
    public LinkTypeDTO LinkType { get; set; } = new LinkTypeDTO();
    public List<LinkedEstablishmentDTO> Establishments { get; set; } = new List<LinkedEstablishmentDTO>();
    public string Description { get; set; } = "";
}