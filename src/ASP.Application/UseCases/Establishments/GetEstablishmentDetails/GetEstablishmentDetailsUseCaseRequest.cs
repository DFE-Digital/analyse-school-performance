namespace ASP.Application.UseCases.Establishments.GetEstablishmentDetails
{
    public class GetEstablishmentDetailsUseCaseRequest
    {
        public string ContentTemplateId { get; set; }

        public GetEstablishmentDetailsUseCaseRequest(string contentTemplateId)
        {
            ContentTemplateId = contentTemplateId;
        }
    }
}
