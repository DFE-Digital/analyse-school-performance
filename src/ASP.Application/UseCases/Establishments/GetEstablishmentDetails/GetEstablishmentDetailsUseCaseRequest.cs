namespace ASP.Application.UseCases.Establishments.GetEstablishmentDetails
{
    public class GetEstablishmentDetailsUseCaseRequest
    {
        public string Urn { get; set; }

        public GetEstablishmentDetailsUseCaseRequest(string urn)
        {
            Urn = urn;
        }
    }
}
