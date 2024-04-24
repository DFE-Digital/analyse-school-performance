namespace ASP.Application.UseCases.GetEstablishmentDetails
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
