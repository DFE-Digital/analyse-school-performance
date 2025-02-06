using ASP.Api.Client;

namespace ASP.Web.Areas.LocalAuthority;

public class LocalAuthoritiesListingModel
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Url { get; set; } = "";

    public static List<LocalAuthoritiesListingModel> FromLocalAuthoritiesListingDto(
        IEnumerable<LookupValueWithCode> localAuthoritiesListingDto,
        Func<string, string?> createLocalAuthorityUrl)
    {
        var result = localAuthoritiesListingDto.Select(x =>
        {
            var model = new LocalAuthoritiesListingModel {
                Code = x.Code,
                Name = x.Name,
                Url = createLocalAuthorityUrl(x.Code) ?? ""
            };
            return model;
        }).ToList();

        return result;
    }
}