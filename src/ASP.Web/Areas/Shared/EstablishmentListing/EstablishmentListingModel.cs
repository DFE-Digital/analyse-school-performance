using ASP.Application.UseCases.Establishments.DTO;

namespace ASP.Web.Areas.Shared.EstablishmentListing;

public class EstablishmentListingModel
{
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public string Urn { get; set; } = "";
    public string LaEstab { get; set; } = "";
    public string Url { get; set; } = "";

    public static List<EstablishmentListingModel> FromEstablishmentListingDto(
        IEnumerable<EstablishmentListingDTO> establishmentListingDto,
        Func<string, string?> createSchoolUrl
    )
    {
        var result = establishmentListingDto.Select(x =>
        {
            var model = new EstablishmentListingModel
            {
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