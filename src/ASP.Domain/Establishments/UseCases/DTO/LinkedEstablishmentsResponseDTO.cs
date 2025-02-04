namespace ASP.Domain.Establishments.UseCases.DTO;

public class LinkedEstablishmentsResponseDTO
{
    public string Urn { get; set; } = "";
    public string Name  { get; set; } = "";
    public List<string> LinkedUrns  { get; set; } = new List<string>();
    public List<LinkResponseDTO> Links  { get; set; } = new List<LinkResponseDTO>();
}