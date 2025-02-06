using ASP.Api.Client.Establishments;

namespace ASP.Web.Areas.School;

public class EstablishmentListingViewModel
{
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public string Urn { get; set; } = "";
    public string LaEstab { get; set; } = "";
    public string Url { get; set; } = "";

    public static List<EstablishmentListingViewModel> FromEstablishmentListingDto(
        IEnumerable<EstablishmentListing> establishmentListingDto,
        Func<string, string?> createSchoolUrl
    )
    {
        var result = establishmentListingDto.Select(x =>
        {
            var model = new EstablishmentListingViewModel {
                Name = x.Name,
                Address = !string.IsNullOrEmpty(x.Address) ? x.Address : "No address available",
                Urn = x.Urn,
                LaEstab = !string.IsNullOrEmpty(x.Laestab) ? x.Laestab : "No data available",
                Url = createSchoolUrl(x.Urn) ?? ""
            };
            return model;
        }).ToList();

        return result;
    }
}