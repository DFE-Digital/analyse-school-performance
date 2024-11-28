using ASP.Application.UseCases.LocalAuthorities.DTO;

namespace ASP.Web.Areas.Shared.LocalAuthorityListing;

public class LocalAuthoritiesListingModel
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    
    public static List<LocalAuthoritiesListingModel> FromLocalAuthoritiesListingDto(
        IEnumerable<LocalAuthorityDTO> localAuthoritiesListingDto)
    {
        var result = localAuthoritiesListingDto.Select(x =>
        {
            var model = new LocalAuthoritiesListingModel
            {
                Code = x.Code,
                Name = x.Name
            };
            return model;
        }).ToList();

        return result;
    }
}