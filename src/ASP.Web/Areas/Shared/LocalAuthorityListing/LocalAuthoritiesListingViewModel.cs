namespace ASP.Web.Areas.Shared.LocalAuthorityListing;

public class LocalAuthoritiesListingViewModel
{
    public List<LocalAuthoritiesListingModel> LocalAuthoritiesListingModel { get; }


    public LocalAuthoritiesListingViewModel(List<LocalAuthoritiesListingModel> localAuthoritiesListingModel)
    {
        LocalAuthoritiesListingModel = localAuthoritiesListingModel;
    }
}