namespace ASP.Domain.Establishments.UseCases.GetLinkedEstablishments;

public class GetLinkedEstablishmentsRequest
{
    public string Urn { get; set; }

    public GetLinkedEstablishmentsRequest(string urn)
    {
        Urn = urn;
    }
}