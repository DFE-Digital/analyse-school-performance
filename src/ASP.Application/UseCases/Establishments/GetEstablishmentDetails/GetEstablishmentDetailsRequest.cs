namespace ASP.Application.UseCases.Establishments.GetEstablishmentDetails
{
    public class GetEstablishmentDetailsRequest
    {
        public string Urn { get; set; }

        public GetEstablishmentDetailsRequest(string urn)
        {
            Urn = urn;
        }
    }
}
