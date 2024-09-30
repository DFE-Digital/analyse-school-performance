namespace ASP.Web.Areas.Shared.EstablishmentListing;

public class EstablishmentListingViewModel
{
    public List<EstablishmentListingModel> EstablishmentListingModel { get; }
    public EstablishmentListingViewModel(List<EstablishmentListingModel> establishmentListingModel)
    {
        EstablishmentListingModel = establishmentListingModel;
    }
}